using GameOff2026.Common.Application;
using GameOff2026.Game.Data.DataStore;
using GameOff2026.Game.Domain.Repository;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.Repository
{
    [TestFixture]
    public sealed class ShopItemRepositoryTests
    {
        [Test]
        public void Find_ExistingId_ReturnsMatchingShopItem()
        {
            var repository = CreateRepository(new ShopItemMaster(2, "Potion", "Restores stamina", 80, true, 20));

            var result = repository.Find(2);

            Assert.AreEqual(2, result.id);
            Assert.AreEqual("Potion", result.name);
            Assert.AreEqual(80, result.price);
        }

        [Test]
        public void Find_UnconfiguredId_ThrowsNotFoundException()
        {
            var repository = CreateRepository(new ShopItemMaster(2, "Potion", "Restores stamina", 80, true, 20));

            var exception = Assert.Throws<QuitExceptionVO>(() => repository.Find(99));

            Assert.AreEqual(ExceptionConfig.NOT_FOUND_SHOP_ITEM, exception.Message);
        }

        [Test]
        public void GetAll_MultipleMasters_ReturnsAllValueObjects()
        {
            var repository = CreateRepository(
                new ShopItemMaster(1, "Sword", "Attack item", 100, true, 10),
                new ShopItemMaster(2, "Potion", "Stamina item", 50, false, 20));

            var result = repository.GetAll();

            Assert.AreEqual(2, result.Length);
            CollectionAssert.AreEquivalent(new[] { 1, 2 }, new[] { result[0].id, result[1].id });
        }

        private static ShopItemRepository CreateRepository(params ShopItemMaster[] masters)
        {
            var builder = new DatabaseBuilder();
            builder.Append(masters);
            return new ShopItemRepository(new MemoryDatabase(builder.Build()));
        }
    }
}