using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using NUnit.Framework;
using UnityEngine;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class EnumExtensionTests
    {
        [TestCase(1, FieldType.Ground)]
        [TestCase(2, FieldType.GroundDoor)]
        [TestCase(4, FieldType.Treasure)]
        public void ToFieldType_ValidValue_ReturnsMatchingEnum(int value, FieldType expected)
        {
            Assert.AreEqual(expected, value.ToFieldType());
        }

        [TestCase(99)]
        public void ToFieldType_InvalidValue_ThrowsQuitException(int value)
        {
            Assert.Throws<QuitExceptionVO>(() => value.ToFieldType());
        }

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

        [TestCase(Direction.Up, 0, 1)]
        [TestCase(Direction.Down, 0, -1)]
        [TestCase(Direction.Left, -1, 0)]
        [TestCase(Direction.Right, 1, 0)]
        public void ToSquareVO_ValidDirection_ReturnsExpectedOffset(Direction direction, int file, int rank)
        {
            var square = direction.ToSquareVO();

            Assert.AreEqual(file, square.file);
            Assert.AreEqual(rank, square.rank);
        }

        [TestCase(Rotation.Angle0, 1, 2, 1, 2)]
        [TestCase(Rotation.Angle90, 0, 0, 2, 0)]
        [TestCase(Rotation.Angle180, 0, 0, 2, 2)]
        [TestCase(Rotation.Angle270, 0, 0, 0, 2)]
        public void Rotate_ValidRotation_ReturnsExpectedCoordinates(Rotation rotation, int file, int rank, int expectedFile, int expectedRank)
        {
            var result = rotation.Rotate(file, rank);

            Assert.AreEqual((expectedFile, expectedRank), result);
        }

        [Test]
        public void Rotate_InvalidRotation_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() => Rotation.None.Rotate(1, 2));
        }
    }
}
