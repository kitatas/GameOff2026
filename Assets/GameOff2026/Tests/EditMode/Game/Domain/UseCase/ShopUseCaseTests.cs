using System.Collections.Generic;
using System.Reflection;
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
        private ShopPriceRateEntity _shopPriceRateEntity;
        private LocaleRepository _localeRepository;
        private FakeAsyncPublisher<ShopVO[]> _publisher;
        private ShopItemRepository _repository;
        private ShopUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _moneyEntity = new MoneyEntity();
            _purchasedItemEntity = new PurchasedItemEntity();
            _shopPriceRateEntity = new ShopPriceRateEntity();
            _shopPriceRateEntity.Reset();
            _localeRepository = new LocaleRepository();
            _publisher = new FakeAsyncPublisher<ShopVO[]>();
            _repository = CreateRepository(
                new ShopItemMaster(1, "Sword", "Attack item", 100, true, 10),
                new ShopItemMaster(2, "Potion", "Stamina item", 50, false, 20));
            _useCase = new ShopUseCase(
                _moneyEntity,
                _purchasedItemEntity,
                _shopPriceRateEntity,
                new ShopPorts(new FakeAsyncSubscriber<ShopVO[]>(), _publisher),
                _localeRepository,
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

        [Test]
        public async Task PublishShopsAsync_WithRaisedPriceAndInsufficientMoney_PublishesCeiledPriceAndNotEnough()
        {
            _shopPriceRateEntity.Set(150);
            _moneyEntity.Add(140);
            SetLocale("Sword", "Attack item", "Attack by {0}");
            SetLocale("Potion", "Stamina item", "Restore {0}");

            await _useCase.PublishShopsAsync(default).AsTask();

            Assert.AreEqual(1, _publisher.PublishCount);
            var sword = System.Array.Find(_publisher.LastPublished, x => x.shopItem.id == 1);
            Assert.AreEqual(150, sword.shopItem.price);
            Assert.IsFalse(sword.isEnoughMoney);
        }

        [Test]
        public void Init_AfterPurchases_ResetPurchasedItems()
        {
            _useCase.Buy(1);
            _useCase.Buy(2);

            _useCase.Init();

            Assert.AreEqual(0, _purchasedItemEntity.Get(1).count);
            Assert.AreEqual(0, _purchasedItemEntity.Get(2).count);
        }

        private void SetLocale(string key, string descriptionKey, string descriptionValue)
        {
            var localeMap = (Dictionary<string, string>)typeof(LocaleRepository)
                .GetField("_localeMap", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(_localeRepository);
            localeMap[key] = key;
            localeMap[descriptionKey] = descriptionValue;
        }

        private static ShopItemRepository CreateRepository(params ShopItemMaster[] masters)
        {
            var builder = new DatabaseBuilder();
            builder.Append(masters);
            return new ShopItemRepository(new MemoryDatabase(builder.Build()));
        }
    }
}