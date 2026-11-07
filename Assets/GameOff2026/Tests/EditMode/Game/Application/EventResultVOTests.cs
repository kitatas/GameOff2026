using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class EventResultVOTests
    {
        [Test]
        public void Constructor_ValidValues_RetainsTypeAndTreasureCount()
        {
            var result = new EventResultVO(FieldType.Treasure, 3);

            Assert.AreEqual(FieldType.Treasure, result.type);
            Assert.AreEqual(3, result.treasureNum);
        }
    }
}
