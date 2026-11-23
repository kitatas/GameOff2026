using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Utility;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Utility
{
    [TestFixture]
    public sealed class FieldHelperTests
    {
        [TestCase(Direction.Up, 3, 6)]
        [TestCase(Direction.Down, 3, 4)]
        [TestCase(Direction.Left, 2, 5)]
        [TestCase(Direction.Right, 4, 5)]
        public void AddDirection_ValidDirection_ReturnsMovedSquare(Direction direction, int file, int rank)
        {
            var square = new SquareVO(3, 5);

            var result = square.AddDirection(direction);

            Assert.AreEqual(file, result.file);
            Assert.AreEqual(rank, result.rank);
        }

        [Test]
        public void AddDirection_NoneDirection_ThrowsQuitException()
        {
            var square = new SquareVO(3, 5);

            Assert.Throws<QuitExceptionVO>(() => square.AddDirection(Direction.None));
        }
    }
}