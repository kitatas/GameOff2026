using System;
using GameOff2026.Game.Data.DataStore;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Repository;
using GameOff2026.Game.Domain.UseCase;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class FloorUseCaseTests
    {
        private FloorEntity _floorEntity;
        private TreasurePriceRateEntity _treasurePriceRateEntity;
        private FloorUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _floorEntity = new FloorEntity();
            _treasurePriceRateEntity = new TreasurePriceRateEntity();
            _treasurePriceRateEntity.Set(100);
            _useCase = new FloorUseCase(_floorEntity, _treasurePriceRateEntity, CreateRepository());
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase)?.Dispose();
        }

        [Test]
        public void Init_AfterEntitiesHaveValues_ResetsFloorAndTreasurePriceRate()
        {
            _floorEntity.Set(5);
            _treasurePriceRateEntity.Set(150);

            _useCase.Init();

            Assert.AreEqual(1, _floorEntity.value);
            Assert.AreEqual(1, _useCase.floor.CurrentValue);
            Assert.AreEqual(100, _treasurePriceRateEntity.value);
        }

        [Test]
        public void Increment_AfterInitialization_IncreasesAndPublishesFloor()
        {
            _useCase.Init();

            _useCase.Increment();
            _useCase.Increment();

            Assert.AreEqual(3, _floorEntity.value);
            Assert.AreEqual(3, _useCase.floor.CurrentValue);
        }

        [Test]
        public void GetTreasurePrice_AfterIncrement_ReturnsPriceForCurrentFloor()
        {
            _useCase.Init();
            _useCase.Increment();

            var result = _useCase.GetTreasurePrice();

            Assert.AreEqual(200, result);
        }

        private static TreasurePriceRepository CreateRepository()
        {
            var builder = new DatabaseBuilder();
            builder.Append(new[]
            {
                new TreasurePriceMaster(1, 100),
                new TreasurePriceMaster(2, 200),
            });
            return new TreasurePriceRepository(new MemoryDatabase(builder.Build()));
        }
    }
}