using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class SoundVOTests
    {
        [Test]
        public void Constructor_AllValues_RetainsData()
        {
            var audio = new SeAudioVO(SeType.None, "SE_Cue");

            var vo = new TestSoundVO(audio, 0.35f, true);

            Assert.AreSame(audio, vo.audio);
            Assert.AreEqual(0.35f, vo.delay);
            Assert.IsTrue(vo.isMute);
        }

        private sealed class TestSoundVO : SoundVO<SeType>
        {
            public TestSoundVO(AudioVO<SeType> audio, float delay, bool isMute)
                : base(audio, delay, isMute)
            {
            }
        }
    }
}

