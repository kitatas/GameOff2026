using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class EnemyEntityTests
    {
        [Test]
        public void Constructor_DayAndNightMaps_ContainsFourEmptySlotsEach()
        {
            var entity = new EnemyEntity();

            Assert.AreEqual(4, entity.Get(DateType.Day).Length);
            Assert.AreEqual(4, entity.Get(DateType.Night).Length);
            Assert.IsNull(entity.Get(DateType.Day)[0]);
            Assert.IsNull(entity.Get(DateType.Night)[0]);
            Assert.AreNotSame(entity.Get(DateType.Day), entity.Get(DateType.Night));
        }

        [Test]
        public void Set_DayEnemySquare_UpdatesOnlyRequestedDateAndIndex()
        {
            var entity = new EnemyEntity();
            var square = new SquareVO(2, 3);

            entity.Set(DateType.Day, 1, square);

            Assert.AreSame(square, entity.Get(DateType.Day)[1]);
            Assert.IsNull(entity.Get(DateType.Day)[0]);
            Assert.IsNull(entity.Get(DateType.Night)[1]);
        }
    }
}