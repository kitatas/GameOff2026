using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class SeSoundVOTests
    {
        [Test]
        public void Constructor_AllValues_RetainsData()
        {
            var audio = new SeAudioVO(SeType.None, "SE_Cue");

            var vo = new SeSoundVO(audio, 0.5f, false);

            Assert.AreSame(audio, vo.audio);
            Assert.AreEqual(0.5f, vo.delay);
            Assert.IsFalse(vo.isMute);
        }
    }
}

