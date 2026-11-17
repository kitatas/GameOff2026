using System;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.UseCase;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class StaminaUseCaseTests
    {
        private StaminaEntity _staminaEntity;
        private MaxStaminaEntity _maxStaminaEntity;
        private StaminaUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _staminaEntity = new StaminaEntity();
            _maxStaminaEntity = new MaxStaminaEntity();
            _useCase = new StaminaUseCase(_staminaEntity, _maxStaminaEntity);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void SetUp_AfterConstruction_SetsMaxAndStaminaToConfiguredValue()
        {
            _useCase.SetUp();

            Assert.AreEqual(PlayerConfig.MAX_STAMINA, _maxStaminaEntity.value);
            Assert.AreEqual(PlayerConfig.MAX_STAMINA, _staminaEntity.value);
            Assert.AreEqual(PlayerConfig.MAX_STAMINA, _useCase.stamina.CurrentValue);
            Assert.AreEqual(PlayerConfig.MAX_STAMINA, _useCase.maxStamina.CurrentValue);
        }

        [Test]
        public void Decrease_AfterSetUp_DecreasesStaminaAndPublishesValue()
        {
            _useCase.SetUp();

            _useCase.Decrease();

            Assert.AreEqual(PlayerConfig.MAX_STAMINA - 1, _staminaEntity.value);
            Assert.AreEqual(PlayerConfig.MAX_STAMINA - 1, _useCase.stamina.CurrentValue);
        }

        [Test]
        public void Refresh_AfterDecrease_RestoresMaxStamina()
        {
            _useCase.SetUp();
            _useCase.Decrease();

            _useCase.Refresh();

            Assert.AreEqual(PlayerConfig.MAX_STAMINA, _staminaEntity.value);
            Assert.AreEqual(PlayerConfig.MAX_STAMINA, _useCase.stamina.CurrentValue);
        }

        [Test]
        public void IsEmpty_AtZero_ReturnsTrue()
        {
            _useCase.SetUp();
            _staminaEntity.Set(0);

            Assert.IsTrue(_useCase.IsEmpty());
        }

        [Test]
        public void IsEmpty_AfterSetUp_ReturnsFalse()
        {
            _useCase.SetUp();

            Assert.IsFalse(_useCase.IsEmpty());
        }
    }
}
