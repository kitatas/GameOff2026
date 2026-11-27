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

        [TestCase(true, 0, false)]
        [TestCase(true, 1, true)]
        [TestCase(false, 1, false)]
        public void IsSoldOut_UniqueAndPurchaseState_ReturnsExpected(bool isUniq, int purchasedCount,
            bool expected)
        {
            var shopItem = new ShopItemVO(1, "Item", "Description", 50, isUniq, 10);
            var purchasedItem = new PurchasedItemVO(1, purchasedCount);

            var vo = new ShopVO(shopItem, purchasedItem, true);

            Assert.AreEqual(expected, vo.isSoldOut);
        }

        [TestCase(true, 0, true, true)]
        [TestCase(true, 1, true, false)]
        [TestCase(false, 1, true, true)]
        [TestCase(true, 0, false, false)]
        public void CanBuy_SoldOutAndMoneyState_ReturnsExpected(bool isUniq, int purchasedCount,
            bool isEnoughMoney, bool expected)
        {
            var shopItem = new ShopItemVO(1, "Item", "Description", 50, isUniq, 10);
            var purchasedItem = new PurchasedItemVO(1, purchasedCount);

            var vo = new ShopVO(shopItem, purchasedItem, isEnoughMoney);

            Assert.AreEqual(expected, vo.canBuy);
        }
    }
}