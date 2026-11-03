using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using NUnit.Framework;
using UnityEngine;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class EnumExtensionTests
    {
        [TestCase(Direction.Up, 0, 1)]
        [TestCase(Direction.Down, 0, -1)]
        [TestCase(Direction.Left, -1, 0)]
        [TestCase(Direction.Right, 1, 0)]
        public void ToVector2_ValidDirection_ReturnsUnitVector(Direction direction, int x, int y)
        {
            var vector = direction.ToVector2();

            Assert.AreEqual(new Vector2Int(x, y), vector);
        }

        [Test]
        public void ToVector2_None_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() => Direction.None.ToVector2());
        }
    }
}
