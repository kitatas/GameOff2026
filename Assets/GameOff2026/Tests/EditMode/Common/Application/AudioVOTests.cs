using System;
using GameOff2026.Common.Application;
using NUnit.Framework;
using UnityEngine;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class AudioVOTests
    {
        [Test]
        public void BgmAudioVO_WithCue_RetainsTypeAndCue()
        {
            var vo = new BgmAudioVO(BgmType.None, "BGM_Cue");

            Assert.AreEqual(BgmType.None, vo.type);
            Assert.AreEqual("BGM_Cue", vo.cue);
            Assert.IsNull(vo.clip);
        }

        [Test]
        public void SeAudioVO_WithCue_RetainsTypeAndCue()
        {
            var vo = new SeAudioVO(SeType.None, "SE_Cue");

            Assert.AreEqual(SeType.None, vo.type);
            Assert.AreEqual("SE_Cue", vo.cue);
            Assert.IsNull(vo.clip);
        }

        [Test]
        public void BgmSoundVO_WithValues_RetainsAudioDelayAndMute()
        {
            var audio = new BgmAudioVO(BgmType.None, "BGM_Cue");

            var vo = new BgmSoundVO(audio, 0.25f, true);

            Assert.AreSame(audio, vo.audio);
            Assert.AreEqual(0.25f, vo.delay);
            Assert.IsTrue(vo.isMute);
        }

        [Test]
        public void SeSoundVO_WithValues_RetainsAudioDelayAndMute()
        {
            var audio = new SeAudioVO(SeType.None, "SE_Cue");

            var vo = new SeSoundVO(audio, 0.5f, false);

            Assert.AreSame(audio, vo.audio);
            Assert.AreEqual(0.5f, vo.delay);
            Assert.IsFalse(vo.isMute);
        }
    }
}
