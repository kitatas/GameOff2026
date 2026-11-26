using GameOff2026.Common.Application;
using GameOff2026.Common.Data.DataStore;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Data.DataStore
{
    [TestFixture]
    public sealed class VolumeDTOTests
    {
        [Test]
        public void Constructor_Default_UsesInitialVolumeAndIsNotMuted()
        {
            var dto = new VolumeDTO();

            Assert.AreEqual(SoundConfig.INIT_VOLUME, dto.value);
            Assert.IsFalse(dto.isMute);
        }

        [Test]
        public void Constructor_VolumeVO_CopiesValues()
        {
            var vo = new VolumeVO(0.35f, true);

            var dto = new VolumeDTO(vo);

            Assert.AreEqual(0.35f, dto.value);
            Assert.IsTrue(dto.isMute);
        }

        [Test]
        public void ToVO_ValidValues_CreatesVolumeVO()
        {
            var dto = new VolumeDTO { value = 0.8f, isMute = true };

            var vo = dto.ToVO();

            Assert.AreEqual(0.8f, vo.value);
            Assert.IsTrue(vo.isMute);
        }
    }
}

