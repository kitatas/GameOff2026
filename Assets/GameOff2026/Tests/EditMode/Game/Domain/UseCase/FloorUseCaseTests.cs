using System;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.UseCase;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class FloorUseCaseTests
    {
        private FloorEntity _floorEntity;
        private FloorUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _floorEntity = new FloorEntity();
            _useCase = new FloorUseCase(_floorEntity);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void Init_AfterEntityHasFloor_ResetsEntityAndPublishesFloorOne()
        {
            _floorEntity.Set(5);

            _useCase.Init();

            Assert.AreEqual(1, _floorEntity.value);
            Assert.AreEqual(1, _useCase.floor.CurrentValue);
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
    }
}
