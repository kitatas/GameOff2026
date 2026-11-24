using System.Reflection;
using GameOff2026.Boot.Application;
using GameOff2026.Boot.Data.DataStore;
using GameOff2026.Common.Application;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;
using UnityEngine;

namespace GameOff2026.Tests.EditMode.Boot.Data.DataStore
{
    [TestFixture]
    public sealed class SplashDataTests
    {
        private SplashData _data;

        [SetUp]
        public void SetUp()
        {
            _data = ScriptableObject.CreateInstance<SplashData>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_data);
        }

        [Test]
        public void ToVO_ValidSerializedValues_CreatesMatchingValueObject()
        {
            SetField("splashType", SplashType.Developer);
            SetField("splashSprite", TestSpriteFactory.Valid);

            var vo = _data.ToVO();

            Assert.AreEqual(SplashType.Developer, _data.type);
            Assert.AreEqual(SplashType.Developer, vo.type);
            Assert.AreSame(TestSpriteFactory.Valid, vo.sprite);
        }

        [Test]
        public void ToVO_NoneSerializedType_ThrowsInvalidSplashException()
        {
            SetField("splashType", SplashType.None);
            SetField("splashSprite", TestSpriteFactory.Valid);

            Assert.Throws<QuitExceptionVO>(() => _data.ToVO());
        }

        private void SetField(string fieldName, object value)
        {
            var field = typeof(SplashData).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, "Expected serialized field '" + fieldName + "' to exist.");
            field.SetValue(_data, value);
        }
    }
}

