using System;
using System.Collections.Generic;
using System.Reflection;
using GameOff2026.Common.Data.DataStore;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.DataStore;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Repository;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;
using UnityEngine;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class EnemyWanderUseCaseTests
    {
        private DateEntity _dateEntity;
        private EnemyEntity _enemyEntity;
        private FieldEntity _fieldEntity;
        private FieldTable _fieldTable;
        private List<FieldData> _fieldDataAssets;
        private EnemyWanderUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _dateEntity = new DateEntity();
            _enemyEntity = new EnemyEntity();
            _fieldEntity = new FieldEntity();
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

            SetField(typeof(BaseTable<FieldData>), _fieldTable, "list", _fieldDataAssets);
            var repository = new FieldRepository(_fieldTable);
            _useCase = new EnemyWanderUseCase(_dateEntity, _enemyEntity, _fieldEntity, repository);
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

        [TestCase(DateType.Day, FieldType.EnemyDay)]
        [TestCase(DateType.Night, FieldType.EnemyNight)]
        public void Wander_EnemyAdjacentToMatchingRoute_MovesEnemyAndSwapsFieldTypes(
            DateType date, FieldType enemyType)
        {
            _dateEntity.Set(date);
            var enemySquares = new[]
            {
                new SquareVO(1, 1),
                new SquareVO(5, 5),
                new SquareVO(6, 6),
                new SquareVO(7, 7),
            };
            for (var i = 0; i < enemySquares.Length; i++)
            {
                _enemyEntity.Set(date, i, enemySquares[i]);
                _fieldEntity.Add(new FieldSquareVO(enemySquares[i],
                    new FieldVO(enemyType, TestSpriteFactory.Valid, 1.0f)));
            }

            var routeSquare = new SquareVO(2, 1);
            _fieldEntity.Add(new FieldSquareVO(routeSquare,
                new FieldVO(FieldType.EnemyRouteActive, TestSpriteFactory.Valid, 1.0f)));

            _useCase.Wander();

            Assert.IsTrue(routeSquare.IsEqual(_enemyEntity.Get(date)[0]));
            Assert.AreSame(enemySquares[1], _enemyEntity.Get(date)[1]);
            Assert.AreSame(enemySquares[2], _enemyEntity.Get(date)[2]);
            Assert.AreSame(enemySquares[3], _enemyEntity.Get(date)[3]);
            Assert.AreEqual(enemyType, _fieldEntity.Find(routeSquare).fieldSquare.field.type);
            Assert.AreEqual(FieldType.EnemyRouteActive, _fieldEntity.Find(enemySquares[0]).fieldSquare.field.type);
        }

        [Test]
        public void Wander_EnemyAdjacentOnlyToInactiveRoute_DoesNotMove()
        {
            _dateEntity.Set(DateType.Day);
            var enemySquare = new SquareVO(1, 1);
            var inactiveRouteSquare = new SquareVO(2, 1);
            _enemyEntity.Set(DateType.Day, 0, enemySquare);
            _fieldEntity.Add(new FieldSquareVO(enemySquare,
                new FieldVO(FieldType.EnemyDay, TestSpriteFactory.Valid, 1.0f)));
            _fieldEntity.Add(new FieldSquareVO(inactiveRouteSquare,
                new FieldVO(FieldType.EnemyRouteDeactive, TestSpriteFactory.Valid, 1.0f)));

            _useCase.Wander();

            Assert.AreSame(enemySquare, _enemyEntity.Get(DateType.Day)[0]);
            Assert.AreEqual(FieldType.EnemyDay, _fieldEntity.Find(enemySquare).fieldSquare.field.type);
            Assert.AreEqual(FieldType.EnemyRouteDeactive,
                _fieldEntity.Find(inactiveRouteSquare).fieldSquare.field.type);
        }

        private FieldData CreateFieldData(FieldType type)
        {
            var fieldData = ScriptableObject.CreateInstance<FieldData>();
            SetField(fieldData, "fieldType", type);
            SetField(fieldData, "sprite", TestSpriteFactory.Valid);
            SetField(fieldData, "weight", 1.0f);
            return fieldData;
        }

        private static void SetField(object target, string fieldName, object value)
        {
            var field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Expected serialized field '{fieldName}' to exist.");
            field.SetValue(target, value);
        }

        private static void SetField(Type declaringType, object target, string fieldName, object value)
        {
            var field = declaringType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Expected serialized field '{fieldName}' to exist.");
            field.SetValue(target, value);
        }
    }
}