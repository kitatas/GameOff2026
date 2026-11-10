using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class DateEntityTests
    {
        [Test]
        public void Constructor_DefaultValue_IsNone()
        {
            var entity = new DateEntity();

            Assert.AreEqual(DateType.None, entity.value);
        }

        [Test]
        public void Reset_AfterNight_SetsDateToDay()
        {
            var entity = new DateEntity();
            entity.Set(DateType.Night);

            entity.Reset();

            Assert.AreEqual(DateType.Day, entity.value);
        }

        [TestCase(DateType.Day, DateType.Night)]
        [TestCase(DateType.Night, DateType.Day)]
        public void Shift_ValidDate_TogglesDate(DateType date, DateType expectedDate)
        {
            var entity = new DateEntity();
            entity.Set(date);

            entity.Shift();

            Assert.AreEqual(expectedDate, entity.value);
        }

        [Test]
        public void Shift_NoneDate_ThrowsAndPreservesDate()
        {
            var entity = new DateEntity();

            Assert.Throws<QuitExceptionVO>(() => entity.Shift());

            Assert.AreEqual(DateType.None, entity.value);
        }
    }
}