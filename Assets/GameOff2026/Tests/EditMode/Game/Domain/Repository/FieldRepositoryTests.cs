using System.Collections.Generic;
using System.Reflection;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.DataStore;
using GameOff2026.Game.Domain.Repository;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;
using UnityEngine;

namespace GameOff2026.Tests.EditMode.Game.Domain.Repository
{
    [TestFixture]
    public sealed class FieldRepositoryTests
    {
        private FieldTable _table;

        [SetUp]
        public void SetUp()
        {
            _table = ScriptableObject.CreateInstance<FieldTable>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_table);
        }

        [Test]
        public void Find_ConfiguredTypeAndPositiveWeight_ReturnsField()
        {
            var data = ScriptableObject.CreateInstance<FieldData>();
            try
            {
                SetField(data, "fieldType", FieldType.Treasure);
                SetField(data, "sprite", TestSpriteFactory.Valid);
                SetField(data, "weight", 2.0f);
                SetTableList(_table, new List<FieldData> { data });

                var repository = new FieldRepository(_table);

                var result = repository.Find(FieldType.Treasure);

                Assert.AreEqual(FieldType.Treasure, result.type);
                Assert.AreSame(TestSpriteFactory.Valid, result.sprite);
                Assert.AreEqual(2.0f, result.weight);
            }
            finally
            {
                Object.DestroyImmediate(data);
            }
        }

        [Test]
        public void Find_UnconfiguredType_ThrowsInvalidFieldException()
        {
            SetTableList(_table, new List<FieldData>());
            var repository = new FieldRepository(_table);

            var exception = Assert.Throws<QuitExceptionVO>(() => repository.Find(FieldType.Block));

            Assert.AreEqual(ExceptionConfig.INVALID_FIELD, exception.Message);
        }

        private static void SetTableList(FieldTable table, List<FieldData> items)
        {
            var field = typeof(GameOff2026.Common.Data.DataStore.BaseTable<FieldData>).GetField("list", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field);
            field.SetValue(table, items);
        }

        private static void SetField(FieldData data, string fieldName, object value)
        {
            var field = typeof(FieldData).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field);
            field.SetValue(data, value);
        }
    }
}

