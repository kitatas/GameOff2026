using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class SeAudioVOTests
    {
        [Test]
        public void Constructor_WithCue_RetainsTypeAndCue()
        {
            var vo = new SeAudioVO(SeType.None, "SE_Cue");

            Assert.AreEqual(SeType.None, vo.type);
            Assert.AreEqual("SE_Cue", vo.cue);
            Assert.IsNull(vo.clip);
        }
    }
}

