using System;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.UseCase;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class TreasureUseCaseTests
    {
        private TreasureEntity _treasureEntity;
        private TreasureUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _treasureEntity = new TreasureEntity();
            _useCase = new TreasureUseCase(_treasureEntity);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void SetUp_EntityHasTreasure_ResetsEntityAndUseCaseValue()
        {
            _treasureEntity.Add(5);

            _useCase.SetUp();

            Assert.AreEqual(0, _treasureEntity.value);
            Assert.AreEqual(0, _useCase.treasure.CurrentValue);
        }

        [Test]
        public void Add_AmountProvided_UpdatesEntityAndUseCaseValue()
        {
            _useCase.SetUp();

            _useCase.Add(4);

            Assert.AreEqual(4, _treasureEntity.value);
            Assert.AreEqual(4, _useCase.treasure.CurrentValue);
        }
    }
}
