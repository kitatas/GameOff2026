using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class SaveVOTests
    {
        [Test]
        public void Constructor_AllValues_RetainsData()
        {
            var user = new LocalUserVO("user-001");
            var master = new VolumeVO(0.8f, false);
            var bgm = new VolumeVO(0.6f, true);
            var se = new VolumeVO(0.4f, false);

            var vo = new SaveVO(user, master, bgm, se);

            Assert.AreSame(user, vo.user);
            Assert.AreSame(master, vo.masterVolume);
            Assert.AreSame(bgm, vo.bgmVolume);
            Assert.AreSame(se, vo.seVolume);
        }
    }
}