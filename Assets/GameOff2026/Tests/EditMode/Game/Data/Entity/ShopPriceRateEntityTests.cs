using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class ShopPriceRateEntityTests
    {
        [Test]
        public void Subtract_PositiveAmount_UpdatesValueAndRate()
        {
            var entity = new ShopPriceRateEntity();
            entity.Reset();

            entity.Subtract(20);

            Assert.AreEqual(80, entity.value);
            Assert.AreEqual(0.8f, entity.rateValue);
        }

        [Test]
        public void Reset_AfterValueChanges_RestoresDefaultRate()
        {
            var entity = new ShopPriceRateEntity();
            entity.Set(60);

            entity.Reset();

            Assert.AreEqual(100, entity.value);
            Assert.AreEqual(1.0f, entity.rateValue);
        }
    }
}
