using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class MoneyEntityTests
    {
        [Test]
        public void Constructor_DefaultValue_IsZero()
        {
            var entity = new MoneyEntity();

            Assert.AreEqual(0, entity.value);
        }

        [Test]
        public void Add_MultipleAmounts_AccumulatesValue()
        {
            var entity = new MoneyEntity();

            entity.Add(10);
            entity.Add(5);

            Assert.AreEqual(15, entity.value);
        }

        [Test]
        public void Reset_AfterAddingMoney_SetsValueToZero()
        {
            var entity = new MoneyEntity();
            entity.Add(10);

            entity.Reset();

            Assert.AreEqual(0, entity.value);
        }
    }
}
