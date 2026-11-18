using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class FloorEntityTests
    {
        [Test]
        public void Constructor_DefaultValue_IsZero()
        {
            var entity = new FloorEntity();

            Assert.AreEqual(0, entity.value);
        }

        [Test]
        public void Reset_AfterChangingFloor_SetsValueToOne()
        {
            var entity = new FloorEntity();
            entity.Set(5);

            entity.Reset();

            Assert.AreEqual(1, entity.value);
        }

        [Test]
        public void Increment_MultipleCalls_IncreasesValueByOneEachTime()
        {
            var entity = new FloorEntity();
            entity.Reset();

            entity.Increment();
            entity.Increment();

            Assert.AreEqual(3, entity.value);
        }
    }
}
