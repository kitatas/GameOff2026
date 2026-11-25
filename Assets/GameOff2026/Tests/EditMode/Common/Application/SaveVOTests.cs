using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class SaveVOTests
    {
        [Test]
        public void Constructor_LocalUser_KeepsLocalUser()
        {
            var user = new LocalUserVO("user-001");

            var vo = new SaveVO(user);

            Assert.AreSame(user, vo.user);
        }
    }
}