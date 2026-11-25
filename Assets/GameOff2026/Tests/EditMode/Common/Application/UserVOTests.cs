using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class UserVOTests
    {
        [Test]
        public void Constructor_LocalUser_KeepsLocalUser()
        {
            var localUser = new LocalUserVO("user-002");

            var vo = new UserVO(localUser);

            Assert.AreSame(localUser, vo.localUser);
        }

        [Test]
        public void Constructor_NullLocalUser_KeepsNull()
        {
            var vo = new UserVO(null);

            Assert.IsNull(vo.localUser);
        }
    }
}

