using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.DataStore;
using GameOff2026.Game.Domain.Repository;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.Repository
{
    [TestFixture]
    public sealed class FieldPatternRepositoryTests
    {
        private static readonly int[] ValidTypes = { 1, 4, 7, 4, 8, 9, 10, 11, 1 };

        [Test]
        public void GetRandom4_DayPatterns_ReturnsFourPatternsForDay()
        {
            var repository = CreateRepository(
                new FieldPatternMaster(1, (int)DateType.Day, ValidTypes),
                new FieldPatternMaster(2, (int)DateType.Day, ValidTypes),
                new FieldPatternMaster(3, (int)DateType.Night, ValidTypes));

            var patterns = repository.GetRandom4(DateType.Day);

            Assert.AreEqual(FieldConfig.LOT_PATTERN_COUNT, patterns.Length);
            foreach (var pattern in patterns)
            {
                Assert.AreEqual(DateType.Day, pattern.date);
                Assert.That(pattern.id, Is.EqualTo(1).Or.EqualTo(2));
                Assert.That(FieldConfig.ROTATIONS, Does.Contain(pattern.rotation));
            }
        }

        [Test]
        public void Get_ExactIdAndDateAndRotation_ReturnsMatchingPattern()
        {
            var repository = CreateRepository(
                new FieldPatternMaster(12, (int)DateType.Night, ValidTypes));

            var pattern = repository.Get(12, DateType.Night, Rotation.Angle270);

            Assert.AreEqual(12, pattern.id);
            Assert.AreEqual(DateType.Night, pattern.date);
            Assert.AreEqual(Rotation.Angle270, pattern.rotation);
            CollectionAssert.AreEqual(new[]
            {
                FieldType.EnemyRouteActive, FieldType.Treasure, FieldType.Empty,
                FieldType.EnemyRouteDeactive, FieldType.EnemyDay, FieldType.Treasure,
                FieldType.Empty, FieldType.EnemyNight, FieldType.Block
            }, pattern.types);
        }

        [TestCase(13, DateType.Night)]
        [TestCase(12, DateType.Day)]
        public void Get_UnconfiguredIdOrDate_ThrowsInvalidFieldPattern(int id, DateType date)
        {
            var repository = CreateRepository(
                new FieldPatternMaster(12, (int)DateType.Night, ValidTypes));

            var exception = Assert.Throws<QuitExceptionVO>(
                () => repository.Get(id, date, Rotation.Angle0));

            Assert.AreEqual(ExceptionConfig.INVALID_FIELD_PATTERN, exception.Message);
        }

        private static FieldPatternRepository CreateRepository(params FieldPatternMaster[] masters)
        {
            var builder = new DatabaseBuilder();
            builder.Append(masters);
            return new FieldPatternRepository(new MemoryDatabase(builder.Build()));
        }
    }
}
