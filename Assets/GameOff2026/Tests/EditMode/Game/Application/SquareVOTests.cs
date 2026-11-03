using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class SquareVOTests
    {
        [Test]
        public void Point_AtConfiguredOrigin_UsesCenteredLocalCoordinates()
        {
            var square = new SquareVO(FieldConfig.MIN_FILE, FieldConfig.MIN_RANK);

            Assert.AreEqual(-2.5f, square.localX);
            Assert.AreEqual(-2.5f, square.localZ);
            Assert.AreEqual(-2.5f, square.point.x);
            Assert.AreEqual(-2.5f, square.point.y);
        }

        [Test]
        public void Point_AtOffsetSquare_UsesFileAndRankOffsets()
        {
            var square = new SquareVO(FieldConfig.MIN_FILE + 2, FieldConfig.MIN_RANK + 4);

            Assert.AreEqual(-0.5f, square.localX);
            Assert.AreEqual(1.5f, square.localZ);
        }

        [Test]
        public void IsEqual_SameCoordinates_ReturnsTrue()
        {
            Assert.IsTrue(new SquareVO(2, 3).IsEqual(new SquareVO(2, 3)));
        }

        [TestCase(1, 3)]
        [TestCase(2, 4)]
        public void IsEqual_DifferentCoordinate_ReturnsFalse(int file, int rank)
        {
            Assert.IsFalse(new SquareVO(2, 3).IsEqual(new SquareVO(file, rank)));
        }
    }
}
