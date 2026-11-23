using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class ShopItemVOTests
    {
        [Test]
        public void Constructor_AllValues_RetainsData()
        {
            var vo = new ShopItemVO(2, "Potion", "Restores stamina", 80, true, 20);

            Assert.AreEqual(2, vo.id);
            Assert.AreEqual("Potion", vo.name);
            Assert.AreEqual("Restores stamina", vo.description);
            Assert.AreEqual(80, vo.price);
            Assert.IsTrue(vo.isUniq);
            Assert.AreEqual(20, vo.effectValue);
        }
    }
}

