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
        private TreasurePriceRateEntity _treasurePriceRateEntity;
        private ItemUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _maxStaminaEntity = new MaxStaminaEntity();
            _treasurePriceRateEntity = new TreasurePriceRateEntity();
            _useCase = new ItemUseCase(_maxStaminaEntity, _treasurePriceRateEntity);
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
        public void ApplyEffect_InvalidItemId_ThrowsInvalidShopItemException()
        {
            var item = new ShopItemVO((int)ShopItemId.None, "Invalid", "Description", 50, true, 10);

            var exception = Assert.Throws<QuitExceptionVO>(() => _useCase.ApplyEffect(item));

            Assert.AreEqual(ExceptionConfig.INVALID_SHOP_ITEM_ID, exception.Message);
        }
    }
}

