using System.Collections.Generic;
using System.Reflection;
using GameOff2026.Boot.Application;
using GameOff2026.Boot.Data.DataStore;
using GameOff2026.Boot.Domain.Repository;
using GameOff2026.Common.Application;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;
using UnityEngine;

namespace GameOff2026.Tests.EditMode.Boot.Domain.Repository
{
    [TestFixture]
    public sealed class SplashRepositoryTests
    {
        private SplashTable _table;

        [SetUp]
        public void SetUp()
        {
            _table = ScriptableObject.CreateInstance<SplashTable>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_table);
        }

        [Test]
        public void Find_ConfiguredType_ReturnsSplashValueObject()
        {
            var data = ScriptableObject.CreateInstance<SplashData>();
            try
            {
                SetField(data, "splashType", SplashType.Developer);
                SetField(data, "splashSprite", TestSpriteFactory.Valid);
                SetTableList(_table, new List<SplashData> { data });

                var repository = new SplashRepository(_table);

                var result = repository.Find(SplashType.Developer);

                Assert.AreEqual(SplashType.Developer, result.type);
                Assert.AreSame(TestSpriteFactory.Valid, result.sprite);
            }
            finally
            {
                Object.DestroyImmediate(data);
            }
        }

        [Test]
        public void Find_UnconfiguredType_ThrowsInvalidSplashException()
        {
            SetTableList(_table, new List<SplashData>());
            var repository = new SplashRepository(_table);

            var exception = Assert.Throws<QuitExceptionVO>(() => repository.Find(SplashType.PlayFab));

            Assert.AreEqual(ExceptionConfig.INVALID_SPLASH, exception.Message);
        }

        private static void SetTableList(SplashTable table, List<SplashData> items)
        {
            var field = typeof(GameOff2026.Common.Data.DataStore.BaseTable<SplashData>).GetField("list", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field);
            field.SetValue(table, items);
        }

        private static void SetField(SplashData data, string fieldName, object value)
        {
            var field = typeof(SplashData).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field);
            field.SetValue(data, value);
        }
    }
}

