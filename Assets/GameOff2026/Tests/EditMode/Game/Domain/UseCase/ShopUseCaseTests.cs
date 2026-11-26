using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Domain.Repository;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.DataStore;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.Repository;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class ShopUseCaseTests
    {
        private MoneyEntity _moneyEntity;
        private PurchasedItemEntity _purchasedItemEntity;
        private FakeAsyncPublisher<ShopVO[]> _publisher;
        private ShopItemRepository _repository;
        private ShopUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _moneyEntity = new MoneyEntity();
            _purchasedItemEntity = new PurchasedItemEntity();
            _publisher = new FakeAsyncPublisher<ShopVO[]>();
            _repository = CreateRepository(
                new ShopItemMaster(1, "Sword", "Attack item", 100, true, 10),
                new ShopItemMaster(2, "Potion", "Stamina item", 50, false, 20));
            _useCase = new ShopUseCase(
                _moneyEntity,
                _purchasedItemEntity,
                new ShopPorts(new FakeAsyncSubscriber<ShopVO[]>(), _publisher),
                new LocaleRepository(),
                _repository);
        }

        [Test]
        public void Buy_ExistingItem_AddsPurchaseAndReturnsItem()
        {
            var result = _useCase.Buy(2);

            Assert.AreEqual(2, result.id);
            Assert.AreEqual(1, _purchasedItemEntity.Get(2).count);
        }

        [Test]
        public void Buy_SameItemTwice_IncrementsPurchasedCount()
        {
            _useCase.Buy(2);

            var result = _useCase.Buy(2);

            Assert.AreEqual(2, result.id);
            Assert.AreEqual(2, _purchasedItemEntity.Get(2).count);
        }

        private static ShopItemRepository CreateRepository(params ShopItemMaster[] masters)
        {
            var builder = new DatabaseBuilder();
            builder.Append(masters);
            return new ShopItemRepository(new MemoryDatabase(builder.Build()));
        }
    }
}