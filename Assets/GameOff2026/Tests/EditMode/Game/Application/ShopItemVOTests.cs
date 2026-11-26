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

        [Test]
        public void Create_WithLocalizedText_PreservesPriceAndEffectAndUsesText()
        {
            var source = new ShopItemVO(2, "Potion", "Stamina item", 80, true, 20);

            var result = ShopItemVO.Create(source, "薬", "スタミナを回復");

            Assert.AreEqual(source.id, result.id);
            Assert.AreEqual("薬", result.name);
            Assert.AreEqual("スタミナを回復", result.description);
            Assert.AreEqual(source.price, result.price);
            Assert.AreEqual(source.isUniq, result.isUniq);
            Assert.AreEqual(source.effectValue, result.effectValue);
        }
    }
}