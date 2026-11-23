using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class PurchasedItemVOTests
    {
        [Test]
        public void Constructor_ValidValues_RetainsData()
        {
            var vo = new PurchasedItemVO(4, 3);

            Assert.AreEqual(4, vo.id);
            Assert.AreEqual(3, vo.count);
        }
    }
}

