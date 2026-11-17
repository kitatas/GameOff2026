using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class MaxStaminaEntityTests
    {
        [Test]
        public void Constructor_DefaultValue_IsZero()
        {
            var entity = new MaxStaminaEntity();

            Assert.AreEqual(0, entity.value);
        }

        [Test]
        public void Set_ValidValue_UpdatesValue()
        {
            var entity = new MaxStaminaEntity();

            entity.Set(100);

            Assert.AreEqual(100, entity.value);
        }
    }
}
