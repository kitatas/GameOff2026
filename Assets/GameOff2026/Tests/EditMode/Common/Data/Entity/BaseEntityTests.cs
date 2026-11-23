using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Data.Entity
{
    [TestFixture]
    public sealed class BaseEntityTests
    {
        [Test]
        public void Set_NewValue_UpdatesValueAndEquality()
        {
            var entity = new TestEntity();

            entity.Set(42);

            Assert.AreEqual(42, entity.value);
            Assert.IsTrue(entity.IsEqual(42));
            Assert.IsFalse(entity.IsEqual(41));
        }

        private sealed class TestEntity : GameOff2026.Common.Data.Entity.BaseEntity<int>
        {
        }
    }
}

