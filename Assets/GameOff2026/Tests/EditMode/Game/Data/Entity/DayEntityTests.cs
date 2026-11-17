using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class DayEntityTests
    {
        [Test]
        public void Reset_AfterChangingDay_SetsDayToOne()
        {
            var entity = new DayEntity();
            entity.Set(5);

            entity.Reset();

            Assert.AreEqual(0, entity.value);
        }

        [Test]
        public void Increment_AfterReset_IncrementsCurrentDay()
        {
            var entity = new DayEntity();
            entity.Reset();

            entity.Increment();

            Assert.AreEqual(1, entity.value);
        }
    }
}