using GameOff2026.Game.Application;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class FieldSquareVOTests
    {
        [Test]
        public void Constructor_ValidValues_RetainsSquareAndField()
        {
            var square = new SquareVO(2, 3);
            var field = new FieldVO(FieldType.Empty, TestSpriteFactory.Valid, 1.0f);

            var fieldSquare = new FieldSquareVO(square, field);

            Assert.AreSame(square, fieldSquare.square);
            Assert.AreSame(field, fieldSquare.field);
        }

        [Test]
        public void Create_CoordinatesAndField_CreatesFieldSquare()
        {
            var field = new FieldVO(FieldType.Empty, TestSpriteFactory.Valid, 1.0f);

            var fieldSquare = FieldSquareVO.Create(4, 5, field);

            Assert.AreEqual(4, fieldSquare.square.file);
            Assert.AreEqual(5, fieldSquare.square.rank);
            Assert.AreSame(field, fieldSquare.field);
        }
    }
}
