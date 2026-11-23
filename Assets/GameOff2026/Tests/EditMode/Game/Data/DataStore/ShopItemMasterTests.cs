using GameOff2026.Game.Data.DataStore;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.DataStore
{
    [TestFixture]
    public sealed class ShopItemMasterTests
    {
        [Test]
        public void ToVO_AllValues_MapsMatchingValueObject()
        {
            var master = new ShopItemMaster(2, "Potion", "Restores stamina", 80, true, 20);

            var vo = master.ToVO();

            Assert.AreEqual(2, vo.id);
            Assert.AreEqual("Potion", vo.name);
            Assert.AreEqual("Restores stamina", vo.description);
            Assert.AreEqual(80, vo.price);
            Assert.IsTrue(vo.isUniq);
            Assert.AreEqual(20, vo.effectValue);
        }
    }
}

