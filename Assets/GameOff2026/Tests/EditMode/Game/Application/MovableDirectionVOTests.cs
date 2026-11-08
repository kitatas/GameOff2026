using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class MovableDirectionVOTests
    {
        [TestCase(Direction.Up, true)]
        [TestCase(Direction.Left, false)]
        public void Constructor_ValidValues_RetainsDirectionAndMovableState(Direction direction, bool isMovable)
        {
            var movableDirection = new MovableDirectionVO(direction, isMovable);

            Assert.AreEqual(direction, movableDirection.direction);
            Assert.AreEqual(isMovable, movableDirection.isMovable);
        }

        [Test]
        public void Constructor_NoneDirection_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() => new MovableDirectionVO(Direction.None, true));
        }
    }
}
