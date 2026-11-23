using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Data.Entity
{
    [TestFixture]
    public sealed class BaseStateEntityTests
    {
        [Test]
        public void Set_EnumValue_UpdatesState()
        {
            var entity = new TestStateEntity();

            entity.Set(TestState.Active);

            Assert.AreEqual(TestState.Active, entity.value);
        }

        private enum TestState
        {
            Inactive,
            Active,
        }

        private sealed class TestStateEntity : GameOff2026.Common.Data.Entity.BaseStateEntity<TestState>
        {
        }
    }
}

