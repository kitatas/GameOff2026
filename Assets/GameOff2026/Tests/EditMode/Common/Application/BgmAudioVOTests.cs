using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class BgmAudioVOTests
    {
        [Test]
        public void Constructor_WithCue_RetainsTypeAndCue()
        {
            var vo = new BgmAudioVO(BgmType.None, "BGM_Cue");

            Assert.AreEqual(BgmType.None, vo.type);
            Assert.AreEqual("BGM_Cue", vo.cue);
            Assert.IsNull(vo.clip);
        }
    }
}

