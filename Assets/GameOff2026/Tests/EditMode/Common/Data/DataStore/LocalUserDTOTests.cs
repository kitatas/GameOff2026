using GameOff2026.Common.Application;
using GameOff2026.Common.Data.DataStore;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Data.DataStore
{
    [TestFixture]
    public sealed class LocalUserDTOTests
    {
        [Test]
        public void Constructor_Default_SetsEmptyId()
        {
            var dto = new LocalUserDTO();

            Assert.AreEqual(string.Empty, dto.id);
        }

        [Test]
        public void Constructor_LocalUserVO_CopiesId()
        {
            var dto = new LocalUserDTO(new LocalUserVO("user-001"));

            Assert.AreEqual("user-001", dto.id);
        }

        [Test]
        public void ToVO_ValidId_CreatesLocalUserVO()
        {
            var dto = new LocalUserDTO { id = "user-002" };

            var vo = dto.ToVO();

            Assert.AreEqual("user-002", vo.id);
        }
    }
}

