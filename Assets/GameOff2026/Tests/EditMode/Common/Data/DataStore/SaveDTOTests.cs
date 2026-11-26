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

        [Test]
        public void Constructor_Default_CreatesVolumeDTOs()
        {
            var dto = new SaveDTO();

            Assert.IsNotNull(dto.masterVolume);
            Assert.IsNotNull(dto.bgmVolume);
            Assert.IsNotNull(dto.seVolume);
        }

        [Test]
        public void ToVO_ValidValues_CreatesSaveVOWithVolumes()
        {
            var dto = new SaveDTO
            {
                user = new LocalUserDTO { id = "user-003" },
                masterVolume = new VolumeDTO { value = 0.8f, isMute = false },
                bgmVolume = new VolumeDTO { value = 0.6f, isMute = true },
                seVolume = new VolumeDTO { value = 0.4f, isMute = false },
            };

            var vo = dto.ToVO();

            Assert.AreEqual("user-003", vo.user.id);
            Assert.AreEqual(0.8f, vo.masterVolume.value);
            Assert.AreEqual(0.6f, vo.bgmVolume.value);
            Assert.IsTrue(vo.bgmVolume.isMute);
            Assert.AreEqual(0.4f, vo.seVolume.value);
        }
    }
}