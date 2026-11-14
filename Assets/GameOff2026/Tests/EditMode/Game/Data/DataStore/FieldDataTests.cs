using System.Reflection;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.DataStore;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;
using UnityEngine;

namespace GameOff2026.Tests.EditMode.Game.Data.DataStore
{
    [TestFixture]
    public sealed class FieldDataTests
    {
        private FieldData _data;

        [SetUp]
        public void SetUp()
        {
            _data = ScriptableObject.CreateInstance<FieldData>();
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(_data);
        }

        [Test]
        public void ToVO_ValidSerializedValues_CreatesMatchingValueObject()
        {
            SetField("fieldType", FieldType.Treasure);
            SetField("sprite", TestSpriteFactory.Valid);
            SetField("weight", 2.5f);

            var field = _data.ToVO();

            Assert.AreEqual(FieldType.Treasure, _data.type);
            Assert.AreEqual(FieldType.Treasure, field.type);
            Assert.AreSame(TestSpriteFactory.Valid, field.sprite);
            Assert.AreEqual(2.5f, field.weight);
        }

        [Test]
        public void ToVO_InvalidSerializedValues_PropagatesValidationException()
        {
            SetField("fieldType", FieldType.Ground);
            SetField("sprite", TestSpriteFactory.Valid);
            SetField("weight", 0.0f);

            Assert.Throws<QuitExceptionVO>(() => _data.ToVO());
        }

        private void SetField(string fieldName, object value)
        {
            var field = typeof(FieldData).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Expected serialized field '{fieldName}' to exist.");
            field.SetValue(_data, value);
        }
    }
}
