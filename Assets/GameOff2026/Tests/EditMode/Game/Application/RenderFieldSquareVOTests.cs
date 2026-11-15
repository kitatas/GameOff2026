using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class RenderFieldSquareVOTests
    {
        [Test]
        public void Constructor_ValidValues_RetainsRenderTypeAndSquares()
        {
            var squares = new[]
            {
                FieldSquareVO.Create(1, 1, new FieldVO(FieldType.Empty, TestSpriteFactory.Valid, 1.0f)),
            };

            var renderFieldSquare = new RenderFieldSquareVO(RenderType.Refresh, squares);

            Assert.AreEqual(RenderType.Refresh, renderFieldSquare.type);
            Assert.AreSame(squares, renderFieldSquare.squares);
        }

        [Test]
        public void Constructor_NoneRenderType_ThrowsQuitException()
        {
            var squares = new[]
            {
                FieldSquareVO.Create(1, 1, new FieldVO(FieldType.Empty, TestSpriteFactory.Valid, 1.0f)),
            };

            Assert.Throws<QuitExceptionVO>(() => new RenderFieldSquareVO(RenderType.None, squares));
        }
    }
}
