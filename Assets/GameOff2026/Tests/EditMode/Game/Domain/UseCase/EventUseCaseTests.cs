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
using UnityEngine;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class EventUseCaseTests
    {
        private FieldEntity _fieldEntity;
        private PlayerEntity _playerEntity;
        private FieldTable _fieldTable;
        private FieldData _groundFieldData;
        private FakeAsyncSubscriber<EventResultVO> _eventResultSubscriber;
        private FakeAsyncPublisher<EventResultVO> _eventResultPublisher;
        private EventUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _fieldEntity = new FieldEntity();
            _playerEntity = new PlayerEntity();
            _fieldTable = ScriptableObject.CreateInstance<FieldTable>();
            _groundFieldData = ScriptableObject.CreateInstance<FieldData>();
            SetField(_groundFieldData, "fieldType", FieldType.Ground);
            SetField(_groundFieldData, "weight", 1.0f);
            SetField(typeof(BaseTable<FieldData>), _fieldTable, "list", new List<FieldData> { _groundFieldData });
            var repository = new FieldRepository(_fieldTable);
            _eventResultSubscriber = new FakeAsyncSubscriber<EventResultVO>();
            _eventResultPublisher = new FakeAsyncPublisher<EventResultVO>();
            var eventPorts = new EventPorts(_eventResultSubscriber, _eventResultPublisher);
            _useCase = new EventUseCase(_fieldEntity, _playerEntity, eventPorts, repository);
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(_groundFieldData);
            UnityEngine.Object.DestroyImmediate(_fieldTable);
        }

        [Test]
        public async Task ApplyAsync_GroundSquare_LeavesFieldUnchanged()
        {
            var fieldSquare = FieldSquareVO.Create(2, 3, new FieldVO(FieldType.Ground, null, 1.0f));
            _fieldEntity.Add(fieldSquare);
            _playerEntity.SetSquare(new SquareVO(2, 3));
            using var source = new CancellationTokenSource();

            await _useCase.ApplyAsync(source.Token).AsTask();

            Assert.AreSame(fieldSquare, _fieldEntity.fieldSquares[0]);
            Assert.AreEqual(1, _eventResultPublisher.PublishCount);
            Assert.AreEqual(FieldType.Ground, _eventResultPublisher.LastPublished.type);
            Assert.AreEqual(0, _eventResultPublisher.LastPublished.treasureNum);
            Assert.AreEqual(source.Token, _eventResultPublisher.tokens[0]);
        }

        [Test]
        public async Task ApplyAsync_TreasureSquare_ReplacesFieldWithGround()
        {
            var square = new SquareVO(2, 3);
            _fieldEntity.Add(new FieldSquareVO(square, new FieldVO(FieldType.Treasure, null, 1.0f)));
            _playerEntity.SetSquare(square);
            using var source = new CancellationTokenSource();

            await _useCase.ApplyAsync(source.Token).AsTask();

            var updatedFieldSquare = _fieldEntity.fieldSquares[0];
            Assert.AreSame(square, updatedFieldSquare.square);
            Assert.AreEqual(FieldType.Ground, updatedFieldSquare.field.type);
            Assert.AreEqual(1, _eventResultPublisher.PublishCount);
            Assert.AreEqual(FieldType.Treasure, _eventResultPublisher.LastPublished.type);
            Assert.AreEqual(1, _eventResultPublisher.LastPublished.treasureNum);
            Assert.AreEqual(source.Token, _eventResultPublisher.tokens[0]);
        }

        [Test]
        public void EventResultSubscriber_AfterConstruction_ExposesPortsSubscriber()
        {
            Assert.AreSame(_eventResultSubscriber, _useCase.eventResult);
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
