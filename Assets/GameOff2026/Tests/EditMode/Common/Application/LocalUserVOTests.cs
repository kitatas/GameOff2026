using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class LocalUserVOTests
    {
        [Test]
        public void Constructor_ValidId_KeepsId()
        {
            var vo = new LocalUserVO("user-001");

            Assert.AreEqual("user-001", vo.id);
        }

        [Test]
        public void Constructor_EmptyId_KeepsEmptyId()
        {
            var vo = new LocalUserVO(string.Empty);

            Assert.AreEqual(string.Empty, vo.id);
        }
    }
}

