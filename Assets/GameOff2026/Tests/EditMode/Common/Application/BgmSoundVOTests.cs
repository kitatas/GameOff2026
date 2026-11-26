using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class BgmSoundVOTests
    {
        [Test]
        public void Constructor_AllValues_RetainsData()
        {
            var audio = new BgmAudioVO(BgmType.None, "BGM_Cue");

            var vo = new BgmSoundVO(audio, 0.25f, true);

            Assert.AreSame(audio, vo.audio);
            Assert.AreEqual(0.25f, vo.delay);
            Assert.IsTrue(vo.isMute);
        }
    }
}

