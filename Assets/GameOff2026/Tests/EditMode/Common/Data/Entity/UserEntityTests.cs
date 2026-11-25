using GameOff2026.Common.Application;
using GameOff2026.Common.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Data.Entity
{
    [TestFixture]
    public sealed class UserEntityTests
    {
        [Test]
        public void Set_UserVO_UpdatesValueAndEquality()
        {
            var entity = new UserEntity();
            var user = new UserVO(new LocalUserVO("user-001"));

            entity.Set(user);

            Assert.AreSame(user, entity.value);
            Assert.IsTrue(entity.IsEqual(user));
        }
    }
}

