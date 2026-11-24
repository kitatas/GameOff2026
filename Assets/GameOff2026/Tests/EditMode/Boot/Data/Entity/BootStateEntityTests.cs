using GameOff2026.Boot.Application;
using GameOff2026.Boot.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Boot.Data.Entity
{
    [TestFixture]
    public sealed class BootStateEntityTests
    {
        [Test]
        public void Set_BootState_UpdatesValue()
        {
            var entity = new BootStateEntity();

            entity.Set(BootState.Splash);

            Assert.AreEqual(BootState.Splash, entity.value);
        }
    }
}

