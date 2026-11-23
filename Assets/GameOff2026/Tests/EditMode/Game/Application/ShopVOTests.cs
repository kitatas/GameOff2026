using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class ShopVOTests
    {
        [Test]
        public void Constructor_AllValues_RetainsData()
        {
            var shopItem = new ShopItemVO(1, "Item", "Description", 50, false, 10);
            var purchasedItem = new PurchasedItemVO(1, 2);

            var vo = new ShopVO(shopItem, purchasedItem, true);

            Assert.AreSame(shopItem, vo.shopItem);
            Assert.AreSame(purchasedItem, vo.purchasedItem);
            Assert.IsTrue(vo.isEnoughMoney);
        }
    }
}

