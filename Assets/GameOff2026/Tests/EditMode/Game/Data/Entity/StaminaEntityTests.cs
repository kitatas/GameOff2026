using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class StaminaEntityTests
    {
        [Test]
        public void Constructor_DefaultValue_IsZero()
        {
            var entity = new StaminaEntity();

            Assert.AreEqual(0, entity.value);
        }

        [Test]
        public void Decrement_OneRemaining_BecomesZero()
        {
            var entity = new StaminaEntity();
            entity.Set(1);

            entity.Decrement();

            Assert.AreEqual(0, entity.value);
            Assert.IsTrue(entity.IsEmpty());
        }

        [Test]
        public void Decrement_MultipleCalls_DecreasesByOneEachTime()
        {
            var entity = new StaminaEntity();
            entity.Set(3);

            entity.Decrement();
            entity.Decrement();

            Assert.AreEqual(1, entity.value);
            Assert.IsFalse(entity.IsEmpty());
        }

        [Test]
        public void IsEmpty_ValueGreaterThanZero_ReturnsFalse()
        {
            var entity = new StaminaEntity();
            entity.Set(1);

            Assert.IsFalse(entity.IsEmpty());
        }

        [Test]
        public void Decrement_AtZero_BecomesNegativeOne()
        {
            var entity = new StaminaEntity();

            entity.Decrement();

            Assert.AreEqual(-1, entity.value);
            Assert.IsFalse(entity.IsEmpty());
        }
    }
}
