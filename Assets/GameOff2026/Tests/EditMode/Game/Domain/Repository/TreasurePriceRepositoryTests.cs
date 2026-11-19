using GameOff2026.Common.Application;
using GameOff2026.Game.Data.DataStore;
using GameOff2026.Game.Domain.Repository;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.Repository
{
    [TestFixture]
    public sealed class TreasurePriceRepositoryTests
    {
        [Test]
        public void Find_ExactFloor_ReturnsMatchingPrice()
        {
            var repository = CreateRepository(
                new TreasurePriceMaster(2, 100),
                new TreasurePriceMaster(4, 200));

            var result = repository.Find(4);

            Assert.AreEqual(4, result.floor);
            Assert.AreEqual(200, result.price);
        }

        [Test]
        public void Find_UnconfiguredFloor_ReturnsClosestLowerPrice()
        {
            var repository = CreateRepository(
                new TreasurePriceMaster(2, 100),
                new TreasurePriceMaster(5, 250));

            var result = repository.Find(4);

            Assert.AreEqual(2, result.floor);
            Assert.AreEqual(100, result.price);
        }

        [Test]
        public void Find_FloorBelowConfiguredPrices_ThrowsNotFoundException()
        {
            var repository = CreateRepository(new TreasurePriceMaster(2, 100));

            var exception = Assert.Throws<QuitExceptionVO>(() => repository.Find(1));

            Assert.AreEqual(ExceptionConfig.NOT_FOUND_TREASURE_PRICE, exception.Message);
        }

        private static TreasurePriceRepository CreateRepository(params TreasurePriceMaster[] masters)
        {
            var builder = new DatabaseBuilder();
            builder.Append(masters);
            return new TreasurePriceRepository(new MemoryDatabase(builder.Build()));
        }
    }
}