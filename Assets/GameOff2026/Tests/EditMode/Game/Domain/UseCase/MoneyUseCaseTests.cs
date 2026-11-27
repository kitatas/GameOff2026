using System;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.UseCase;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class MoneyUseCaseTests
    {
        private MoneyEntity _moneyEntity;
        private ShopPriceRateEntity _shopPriceRateEntity;
        private MoneyUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _moneyEntity = new MoneyEntity();
            _shopPriceRateEntity = new ShopPriceRateEntity();
            _useCase = new MoneyUseCase(_moneyEntity, _shopPriceRateEntity);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void Init_AfterEntityHasMoney_ResetsEntityAndPriceRateAndPublishesZero()
        {
            _moneyEntity.Add(10);
            _shopPriceRateEntity.Set(80);

            _useCase.Init();

            Assert.AreEqual(0, _moneyEntity.value);
            Assert.AreEqual(100, _shopPriceRateEntity.value);
            Assert.AreEqual(0, _useCase.money.CurrentValue);
        }

        [Test]
        public void Add_AfterInitialization_AccumulatesAndPublishesMoney()
        {
            _useCase.Init();

            _useCase.Add(10);
            _useCase.Add(5);

            Assert.AreEqual(15, _moneyEntity.value);
            Assert.AreEqual(15, _useCase.money.CurrentValue);
        }

        [Test]
        public void Pay_WithShopDiscount_CeilsAndSubtractsDiscountedPayment()
        {
            _useCase.Init();
            _useCase.Add(100);
            _shopPriceRateEntity.Set(80);

            _useCase.Pay(51);

            Assert.AreEqual(59, _moneyEntity.value);
            Assert.AreEqual(59, _useCase.money.CurrentValue);
        }
    }
}