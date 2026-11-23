using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class TreasurePriceRateEntityTests
    {
        [Test]
        public void Add_PositiveAmount_UpdatesValueAndRate()
        {
            var entity = new TreasurePriceRateEntity();

            entity.Add(25);

            Assert.AreEqual(25, entity.value);
            Assert.AreEqual(0.25f, entity.rateValue);
        }

        [Test]
        public void Set_NegativeAmount_AllowsNegativeRate()
        {
            var entity = new TreasurePriceRateEntity();

            entity.Set(-50);

            Assert.AreEqual(-50, entity.value);
            Assert.AreEqual(-0.5f, entity.rateValue);
        }

        [Test]
        public void Reset_AfterValueChanges_RestoresDefaultRate()
        {
            var entity = new TreasurePriceRateEntity();
            entity.Set(150);

            entity.Reset();

            Assert.AreEqual(100, entity.value);
            Assert.AreEqual(1.0f, entity.rateValue);
        }
    }
}