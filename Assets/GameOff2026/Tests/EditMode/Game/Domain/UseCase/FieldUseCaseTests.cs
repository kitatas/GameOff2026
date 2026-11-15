using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Data.DataStore;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.DataStore;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.Repository;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class FieldUseCaseTests
    {
        private DateEntity _dateEntity;
        private EnemyEntity _enemyEntity;
        private FieldEntity _fieldEntity;
        private FieldPatternEntity _fieldPatternEntity;
        private FieldTable _fieldTable;
        private List<FieldData> _fieldDataAssets;
        private TextAsset _memoryFile;
        private FakeAsyncSubscriber<RenderFieldSquareVO> _subscriber;
        private FakeAsyncPublisher<RenderFieldSquareVO> _publisher;
        private FieldUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _dateEntity = new DateEntity();
            _enemyEntity = new EnemyEntity();
            _fieldEntity = new FieldEntity();
            _fieldPatternEntity = new FieldPatternEntity();
            _fieldTable = ScriptableObject.CreateInstance<FieldTable>();
            _fieldDataAssets = new List<FieldData>();
            foreach (var type in new[]
                     {
                         FieldType.Empty, FieldType.Treasure, FieldType.Block, FieldType.EnemyDay,
                         FieldType.EnemyNight, FieldType.EnemyRouteActive, FieldType.EnemyRouteDeactive,
                     })
            {
                _fieldDataAssets.Add(CreateFieldData(type));
            }

            SetField(typeof(BaseTable<FieldData>), _fieldTable, "list",
                _fieldDataAssets);
            // 実際の MasterMemory データを使い、FieldUseCase から Repository までを通して検証する。
            _memoryFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Externals/Binary/MasterMemory.bytes");
            Assert.IsNotNull(_memoryFile);
            _subscriber = new FakeAsyncSubscriber<RenderFieldSquareVO>();
            _publisher = new FakeAsyncPublisher<RenderFieldSquareVO>();
            var ports = new FieldPorts(_subscriber, _publisher);
            var repository = new FieldRepository(_fieldTable);
            var patternRepository = new FieldPatternRepository(new MemoryDatabase(_memoryFile.bytes));
            _useCase = new FieldUseCase(_dateEntity, _enemyEntity, _fieldEntity, _fieldPatternEntity, ports, repository,
                patternRepository);
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var fieldData in _fieldDataAssets)
            {
                UnityEngine.Object.DestroyImmediate(fieldData);
            }

            UnityEngine.Object.DestroyImmediate(_fieldTable);
        }

        [Test]
        public async Task BuildFieldAsync_ValidMasterData_BuildsFieldAndPublishesRefresh()
        {
            _dateEntity.Reset();
            _fieldEntity.Add(FieldSquareVO.Create(1, 1,
                new FieldVO(FieldType.Treasure, TestSpriteFactory.Valid, 1.0f)));
            using var source = new CancellationTokenSource();

            await _useCase.BuildFieldAsync(source.Token).AsTask();

            var squares = _fieldEntity.fieldSquares;
            var renderMessage = _publisher.LastPublished;
            Assert.AreEqual(FieldConfig.MAX_FILE * FieldConfig.MAX_RANK, squares.Length);
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
            Assert.AreEqual(RenderType.Refresh, renderMessage.type);
            Assert.AreEqual(squares.Length, renderMessage.squares.Length);
            for (var i = 0; i < squares.Length; i++)
            {
                Assert.AreSame(squares[i], renderMessage.squares[i]);
            }

            for (var i = 0; i < FieldConfig.MAX_FILE * FieldConfig.MAX_RANK; i++)
            {
                Assert.AreEqual(i % FieldConfig.MAX_FILE + 1, squares[i].square.file);
                Assert.AreEqual(i / FieldConfig.MAX_FILE + 1, squares[i].square.rank);
            }

            AssertEnemyPositionsTracked(squares, DateType.Day, FieldType.EnemyDay);
            AssertEnemyPositionsTracked(squares, DateType.Night, FieldType.EnemyRouteDeactive);
        }

        [Test]
        public async Task ShiftAsync_DayDate_ShiftsRoutesAndKeepsNightEnemiesHidden()
        {
            _dateEntity.Set(DateType.Day);
            var routeSquare = new SquareVO(1, 1);
            var nightEnemySquare = new SquareVO(2, 1);
            _fieldEntity.Add(new FieldSquareVO(routeSquare,
                new FieldVO(FieldType.EnemyRouteActive, TestSpriteFactory.Valid, 1.0f)));
            _fieldEntity.Add(new FieldSquareVO(nightEnemySquare,
                new FieldVO(FieldType.EnemyRouteDeactive, TestSpriteFactory.Valid, 1.0f)));
            _enemyEntity.Set(DateType.Night, 0, nightEnemySquare);
            using var source = new CancellationTokenSource();

            await _useCase.ShiftAsync(source.Token).AsTask();

            Assert.AreEqual(FieldType.EnemyRouteDeactive,
                _fieldEntity.Find(routeSquare).fieldSquare.field.type);
            Assert.AreEqual(FieldType.EnemyRouteDeactive,
                _fieldEntity.Find(nightEnemySquare).fieldSquare.field.type);
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
            Assert.AreEqual(RenderType.Refresh, _publisher.LastPublished.type);
        }

        [Test]
        public async Task ShiftAsync_NightDate_ShiftsRoutesAndRevealsNightEnemies()
        {
            _dateEntity.Set(DateType.Night);
            var routeSquare = new SquareVO(1, 1);
            var nightEnemySquare = new SquareVO(2, 1);
            _fieldEntity.Add(new FieldSquareVO(routeSquare,
                new FieldVO(FieldType.EnemyRouteDeactive, TestSpriteFactory.Valid, 1.0f)));
            _fieldEntity.Add(new FieldSquareVO(nightEnemySquare,
                new FieldVO(FieldType.EnemyRouteActive, TestSpriteFactory.Valid, 1.0f)));
            _enemyEntity.Set(DateType.Night, 0, nightEnemySquare);
            using var source = new CancellationTokenSource();

            await _useCase.ShiftAsync(source.Token).AsTask();

            Assert.AreEqual(FieldType.EnemyRouteActive,
                _fieldEntity.Find(routeSquare).fieldSquare.field.type);
            Assert.AreEqual(FieldType.EnemyNight,
                _fieldEntity.Find(nightEnemySquare).fieldSquare.field.type);
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
            Assert.AreEqual(RenderType.Refresh, _publisher.LastPublished.type);
        }

        [Test]
        public async Task PublishRenderAsync_FieldSquaresExist_PublishesRefreshWithSquaresAndToken()
        {
            var first = FieldSquareVO.Create(2, 3, new FieldVO(FieldType.Empty, TestSpriteFactory.Valid, 1.0f));
            var second = FieldSquareVO.Create(4, 5, new FieldVO(FieldType.Treasure, TestSpriteFactory.Valid, 2.0f));
            _fieldEntity.Add(first);
            _fieldEntity.Add(second);
            using var source = new CancellationTokenSource();

            await _useCase.PublishRenderAsync(source.Token).AsTask();

            var renderMessage = _publisher.LastPublished;
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
            Assert.AreEqual(RenderType.Refresh, renderMessage.type);
            Assert.AreEqual(2, renderMessage.squares.Length);
            Assert.AreSame(first, renderMessage.squares[0]);
            Assert.AreSame(second, renderMessage.squares[1]);
        }

        [Test]
        public void RenderField_AfterConstruction_ExposesPortsSubscriber()
        {
            Assert.AreSame(_subscriber, _useCase.renderField);
        }

        private FieldData CreateFieldData(FieldType type)
        {
            var fieldData = ScriptableObject.CreateInstance<FieldData>();
            SetField(fieldData, "fieldType", type);
            SetField(fieldData, "sprite", TestSpriteFactory.Valid);
            SetField(fieldData, "weight", 1.0f);
            return fieldData;
        }

        private void AssertEnemyPositionsTracked(FieldSquareVO[] squares, DateType date, FieldType fieldType)
        {
            var enemies = _enemyEntity.Get(date);
            for (var i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] == null) continue;

                var found = false;
                foreach (var fieldSquare in squares)
                {
                    if (fieldSquare.field.type != fieldType || !fieldSquare.square.IsEqual(enemies[i])) continue;

                    var file = fieldSquare.square.file - 1;
                    var rank = fieldSquare.square.rank - 1;
                    var patternIndex = rank / FieldConfig.HALF_RANK +
                                       (file / FieldConfig.HALF_FILE) *
                                       (FieldConfig.MAX_FILE / FieldConfig.HALF_FILE);
                    if (patternIndex != i) continue;

                    found = true;
                    break;
                }

                Assert.IsTrue(found, $"Enemy at index {i} must match an enemy field square.");
            }
        }

        private static void SetField(object target, string fieldName, object value)
        {
            SetField(target.GetType(), target, fieldName, value);
        }

        private static void SetField(Type declaringType, object target, string fieldName, object value)
        {
            var field = declaringType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Expected serialized field '{fieldName}' to exist.");
            field.SetValue(target, value);
        }
    }
}
