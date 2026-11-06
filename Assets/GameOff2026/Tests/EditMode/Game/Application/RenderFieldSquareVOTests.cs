using GameOff2026.Game.Application;
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
                FieldSquareVO.Create(1, 1, new FieldVO(FieldType.Ground, null, 1.0f)),
            };

            var renderFieldSquare = new RenderFieldSquareVO(RenderType.Refresh, squares);

            Assert.AreEqual(RenderType.Refresh, renderFieldSquare.type);
            Assert.AreSame(squares, renderFieldSquare.squares);
        }
    }
}
