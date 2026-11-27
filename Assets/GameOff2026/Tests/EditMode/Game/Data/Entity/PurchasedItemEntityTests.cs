using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class PurchasedItemEntityTests
    {
        [Test]
        public void Get_UnpurchasedItem_ReturnsZeroCount()
        {
            var entity = new PurchasedItemEntity();

            var result = entity.Get(7);

            Assert.AreEqual(7, result.id);
            Assert.AreEqual(0, result.count);
        }

        [Test]
        public void Add_SameItemTwice_ReturnsIncrementedCount()
        {
            var entity = new PurchasedItemEntity();

            entity.Add(7);
            entity.Add(7);

            var result = entity.Get(7);
            Assert.AreEqual(7, result.id);
            Assert.AreEqual(2, result.count);
        }

        [Test]
        public void Get_DifferentItems_KeepsSeparateCounts()
        {
            var entity = new PurchasedItemEntity();

            entity.Add(1);
            entity.Add(2);

            Assert.AreEqual(1, entity.Get(1).count);
            Assert.AreEqual(1, entity.Get(2).count);
            Assert.AreEqual(0, entity.Get(3).count);
        }

        [Test]
        public void Get_ThroughBlockEnum_ReturnsIdAndCount()
        {
            var entity = new PurchasedItemEntity();

            entity.Add((int)ShopItemId.ThroughBlock);

            var result = entity.Get(ShopItemId.ThroughBlock);
            Assert.AreEqual((int)ShopItemId.ThroughBlock, result.id);
            Assert.AreEqual(1, result.count);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Has_ThroughBlock_ReturnsPurchaseState(bool isPurchased)
        {
            var entity = new PurchasedItemEntity();
            if (isPurchased)
            {
                entity.Add((int)ShopItemId.ThroughBlock);
            }

            Assert.AreEqual(isPurchased, entity.Has(ShopItemId.ThroughBlock));
        }

        [Test]
        public void Clear_AfterPurchases_ResetAllItemsToZeroCount()
        {
            var entity = new PurchasedItemEntity();
            entity.Add(1);
            entity.Add(2);
            entity.Add(2);

            entity.Clear();

            Assert.AreEqual(0, entity.Get(1).count);
            Assert.AreEqual(0, entity.Get(2).count);
        }
    }
}