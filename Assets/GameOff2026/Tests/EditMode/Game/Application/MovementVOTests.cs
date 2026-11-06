using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class MovementVOTests
    {
        [Test]
        public void Constructor_ValidValues_RetainsSquareAndDuration()
        {
            var square = new SquareVO(2, 4);

            var movement = new MovementVO(square, 0.5f);

            Assert.AreSame(square, movement.square);
            Assert.AreEqual(0.5f, movement.duration);
        }
    }
}
