using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class EventResultVOTests
    {
        [Test]
        public void Constructor_ValidValues_RetainsAllValues()
        {
            var result = new EventResultVO(FieldType.Treasure, 3, true);

            Assert.AreEqual(FieldType.Treasure, result.type);
            Assert.AreEqual(3, result.treasureNum);
            Assert.IsTrue(result.isGoal);
        }

        [Test]
        public void Constructor_NoneType_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() => new EventResultVO(FieldType.None, 3, false));
        }
    }
}
