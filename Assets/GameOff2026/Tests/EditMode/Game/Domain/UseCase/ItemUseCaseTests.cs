using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.UseCase;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class ItemUseCaseTests
    {
        private MaxStaminaEntity _maxStaminaEntity;
        private ShopPriceRateEntity _shopPriceRateEntity;
        private TreasurePriceRateEntity _treasurePriceRateEntity;
        private ItemUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _maxStaminaEntity = new MaxStaminaEntity();
            _shopPriceRateEntity = new ShopPriceRateEntity();
            _shopPriceRateEntity.Reset();
            _treasurePriceRateEntity = new TreasurePriceRateEntity();
            _useCase = new ItemUseCase(_maxStaminaEntity, _shopPriceRateEntity, _treasurePriceRateEntity);
        }

        [Test]
        public void ApplyEffect_MaxStaminaItem_AddsEffectToMaxStamina()
        {
            var item = new ShopItemVO((int)ShopItemId.MaxStamina, "Max stamina", "Description", 50, true, 5);

            _useCase.ApplyEffect(item);

            Assert.AreEqual(5, _maxStaminaEntity.value);
            Assert.AreEqual(0, _treasurePriceRateEntity.value);
        }

        [Test]
        public void ApplyEffect_TreasurePriceRateItem_AddsEffectToRate()
        {
            var item = new ShopItemVO((int)ShopItemId.TreasurePriceRate, "Treasure rate", "Description", 50, true, 25);

            _useCase.ApplyEffect(item);

            Assert.AreEqual(0, _maxStaminaEntity.value);
            Assert.AreEqual(25, _treasurePriceRateEntity.value);
        }

        [Test]
        public void ApplyEffect_ShopPriceRateItem_SubtractsEffectFromShopPriceRate()
        {
            var item = new ShopItemVO((int)ShopItemId.ShopPriceRate, "Shop rate", "Description", 50, true, 20);

            _useCase.ApplyEffect(item);

            Assert.AreEqual(80, _shopPriceRateEntity.value);
            Assert.AreEqual(0.8f, _shopPriceRateEntity.rateValue);
        }

        [Test]
        public void ApplyEffect_InvalidItemId_ThrowsInvalidShopItemException()
        {
            var item = new ShopItemVO((int)ShopItemId.None, "Invalid", "Description", 50, true, 10);

            var exception = Assert.Throws<QuitExceptionVO>(() => _useCase.ApplyEffect(item));

            Assert.AreEqual(ExceptionConfig.INVALID_SHOP_ITEM_ID, exception.Message);
        }

        [Test]
        public void ApplyEffect_ThroughBlockItem_PreservesExistingEffects()
        {
            var item = new ShopItemVO((int)ShopItemId.ThroughBlock, "Through block", "Description", 50, true, 1);

            _useCase.ApplyEffect(item);

            Assert.AreEqual(0, _maxStaminaEntity.value);
            Assert.AreEqual(0, _treasurePriceRateEntity.value);
            Assert.AreEqual(100, _shopPriceRateEntity.value);
        }
    }
}