using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class TreasureEntityTests
    {
        [Test]
        public void Constructor_DefaultValue_IsZero()
        {
            var entity = new TreasureEntity();

            Assert.AreEqual(0, entity.value);
        }

        [Test]
        public void Add_AmountProvided_AddsAmountToValue()
        {
            var entity = new TreasureEntity();

            entity.Add(3);
            entity.Add(2);

            Assert.AreEqual(5, entity.value);
        }

        [Test]
        public void Reset_AfterAddingAmount_SetsValueToZero()
        {
            var entity = new TreasureEntity();
            entity.Add(3);

            entity.Reset();

            Assert.AreEqual(0, entity.value);
        }
    }
}
