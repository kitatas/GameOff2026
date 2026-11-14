using System.Collections.Generic;
using System.Reflection;
using GameOff2026.Common.Data.DataStore;
using GameOff2026.Game.Data.DataStore;
using NUnit.Framework;
using UnityEngine;

namespace GameOff2026.Tests.EditMode.Game.Data.DataStore
{
    [TestFixture]
    public sealed class FieldTableTests
    {
        private FieldTable _table;
        private FieldData _first;
        private FieldData _second;

        [SetUp]
        public void SetUp()
        {
            _table = ScriptableObject.CreateInstance<FieldTable>();
            _first = ScriptableObject.CreateInstance<FieldData>();
            _second = ScriptableObject.CreateInstance<FieldData>();
            var listField = typeof(BaseTable<FieldData>).GetField("list", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(listField);
            listField.SetValue(_table, new List<FieldData> { _first, _second });
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(_table);
            UnityEngine.Object.DestroyImmediate(_first);
            UnityEngine.Object.DestroyImmediate(_second);
        }

        [Test]
        public void All_ContainsConfiguredFieldData()
        {
            CollectionAssert.AreEqual(new[] { _first, _second }, _table.all);
        }
    }
}
