using GameOff2026.Game.Data.DataStore;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.DataStore
{
    [TestFixture]
    public sealed class TreasurePriceMasterTests
    {
        [Test]
        public void ToVO_MapsFloorAndPrice()
        {
            var master = new TreasurePriceMaster(3, 125);

            var result = master.ToVO();

            Assert.AreEqual(3, result.floor);
            Assert.AreEqual(125, result.price);
        }
    }
}