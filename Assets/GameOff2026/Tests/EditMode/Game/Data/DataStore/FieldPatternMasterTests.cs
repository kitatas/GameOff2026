using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.DataStore;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.DataStore
{
    [TestFixture]
    public sealed class FieldPatternMasterTests
    {
        [Test]
        public void ToVO_Angle0Rotation_CreatesMatchingPattern()
        {
            var types = new[] { 1, 2, 3, 4, 5, 6, 7, 1, 2 };
            var master = new FieldPatternMaster(12, (int)DateType.Night, types);

            var pattern = master.ToVO(Rotation.Angle0);

            Assert.AreEqual(12, master.Id);
            Assert.AreEqual((int)DateType.Night, master.Date);
            Assert.AreSame(types, master.Types);
            Assert.AreEqual(12, pattern.id);
            Assert.AreEqual(DateType.Night, pattern.date);
            CollectionAssert.AreEqual(new[]
            {
                FieldType.Ground, FieldType.GroundDoor, FieldType.GroundWall,
                FieldType.Treasure, FieldType.Wall, FieldType.WallDoor,
                FieldType.Block, FieldType.Ground, FieldType.GroundDoor,
            }, pattern.types);
            Assert.AreEqual(Rotation.Angle0, pattern.rotation);
        }

        [Test]
        public void ToVO_DefaultRotation_UsesAConfiguredRotation()
        {
            var master = new FieldPatternMaster(3, (int)DateType.Day, new[] { 1, 2, 3, 4, 5, 6, 7, 1, 2 });

            var pattern = master.ToVO();

            CollectionAssert.Contains(FieldConfig.ROTATIONS, pattern.rotation);
            Assert.AreEqual(3, pattern.id);
            Assert.AreEqual(DateType.Day, pattern.date);
        }
    }
}
