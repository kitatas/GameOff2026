using System;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class FieldPatternVOTests
    {
        [Test]
        public void Constructor_ValidPattern_StoresIdDateAndRotatedFieldTypes()
        {
            var pattern = new FieldPatternVO(7, 1, new[] { 1, 4, 7, 8, 9, 10, 11, 1, 4 });

            Assert.AreEqual(7, pattern.id);
            Assert.AreEqual(DateType.Day, pattern.date);
            Assert.AreEqual(FieldConfig.HALF_FILE * FieldConfig.HALF_RANK, pattern.types.Length);
            Assert.IsTrue(Array.TrueForAll(pattern.types, type => type != FieldType.None));
        }

        [Test]
        public void Constructor_Angle90Rotation_RearrangesFieldTypes()
        {
            var pattern = new FieldPatternVO(7, 1, new[] { 1, 4, 7, 8, 9, 10, 11, 1, 4 }, Rotation.Angle90);

            CollectionAssert.AreEqual(
                new[]
                {
                    FieldType.Block, FieldType.NightEnemy, FieldType.Treasure,
                    FieldType.Treasure, FieldType.DayEnemyRoute, FieldType.Empty,
                    FieldType.Empty, FieldType.DayEnemy, FieldType.NightEnemyRoute
                },
                pattern.types);
        }

        [Test]
        public void Constructor_InvalidLength_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() => new FieldPatternVO(1, 1, new[] { 1, 4, 7, 8, 9, 10, 11, 1 }));
        }

        [Test]
        public void Constructor_InvalidDate_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() => new FieldPatternVO(1, 0, new[] { 1, 4, 7, 8, 9, 10, 11, 1, 4 }));
        }

        [Test]
        public void Constructor_UndefinedDate_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() =>
                new FieldPatternVO(1, 99, new[] { 1, 4, 7, 8, 9, 10, 11, 1, 4 }));
        }

        [Test]
        public void Constructor_UndefinedFieldType_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() =>
                new FieldPatternVO(1, 1, new[] { 1, 4, 7, 8, 9, 10, 11, 99, 4 }));
        }

        [Test]
        public void Constructor_UndefinedRotation_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() =>
                new FieldPatternVO(1, 1, new[] { 1, 4, 7, 8, 9, 10, 11, 1, 4 }, (Rotation)99));
        }

        [Test]
        public void Constructor_NoneRotation_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() =>
                new FieldPatternVO(1, 1, new[] { 1, 4, 7, 8, 9, 10, 11, 1, 4 }, Rotation.None));
        }
    }
}
