using GameOff2026.Common.Data.DataStore;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Data.DataStore
{
    [TestFixture]
    public sealed class SaveDTOTests
    {
        [Test]
        public void Constructor_Default_CreatesLocalUserDTO()
        {
            var dto = new SaveDTO();

            Assert.IsNotNull(dto.user);
            Assert.AreEqual(string.Empty, dto.user.id);
        }

        [Test]
        public void ToVO_ValidUser_CreatesSaveVO()
        {
            var dto = new SaveDTO
            {
                user = new LocalUserDTO { id = "user-003" }
            };

            var vo = dto.ToVO();

            Assert.AreEqual("user-003", vo.user.id);
        }
    }
}