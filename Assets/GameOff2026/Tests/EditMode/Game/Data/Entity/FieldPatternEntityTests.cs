using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class FieldPatternEntityTests
    {
        [Test]
        public void SetUp_ExpectedPatternCount_StoresPatternsByIndex()
        {
            var entity = new FieldPatternEntity();
            var patterns = CreatePatterns();

            entity.SetUp(patterns);

            for (var i = 0; i < patterns.Length; i++)
            {
                Assert.AreSame(patterns[i], entity.Get(i));
            }
        }

        [Test]
        public void SetUp_WrongPatternCount_ThrowsQuitException()
        {
            var entity = new FieldPatternEntity();

            Assert.Throws<QuitExceptionVO>(() => entity.SetUp(new FieldPatternVO[FieldConfig.LOT_PATTERN_COUNT - 1]));
        }

        [TestCase(-1)]
        [TestCase(FieldConfig.LOT_PATTERN_COUNT)]
        public void Get_UnsetOrOutOfRangeIndex_ThrowsQuitException(int index)
        {
            var entity = new FieldPatternEntity();

            Assert.Throws<QuitExceptionVO>(() => entity.Get(index));
        }

        [Test]
        public void Set_ExistingIndex_ReplacesPattern()
        {
            var entity = new FieldPatternEntity();
            var original = CreatePattern(1);
            var replacement = CreatePattern(2);
            entity.Set(0, original);

            entity.Set(0, replacement);

            Assert.AreSame(replacement, entity.Get(0));
        }

        private static FieldPatternVO[] CreatePatterns()
        {
            var patterns = new FieldPatternVO[FieldConfig.LOT_PATTERN_COUNT];
            for (var i = 0; i < patterns.Length; i++)
            {
                patterns[i] = CreatePattern(i);
            }

            return patterns;
        }

        private static FieldPatternVO CreatePattern(int id)
        {
            return new FieldPatternVO(id, (int)DateType.Day, new[] { 1, 2, 3, 4, 5, 6, 7, 1, 2 },
                Rotation.Angle0);
        }
    }
}
