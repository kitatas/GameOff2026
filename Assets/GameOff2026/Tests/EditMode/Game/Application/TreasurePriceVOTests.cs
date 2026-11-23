using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class TreasurePriceVOTests
    {
        [Test]
        public void Constructor_ValidValues_RetainsData()
        {
            var vo = new TreasurePriceVO(3, 125);

            Assert.AreEqual(3, vo.floor);
            Assert.AreEqual(125, vo.price);
        }
    }
}

