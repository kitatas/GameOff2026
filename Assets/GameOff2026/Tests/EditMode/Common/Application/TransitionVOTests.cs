using System;
using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class TransitionVOTests
    {
        [TestCase(Fade.In, 0.5f)]
        [TestCase(Fade.Out, 1.0f)]
        [TestCase(Fade.InOut, 2.0f)]
        public void Constructor_ValidValues_KeepsValues(Fade fade, float duration)
        {
            var vo = new TransitionVO(fade, duration);

            Assert.AreEqual(fade, vo.fade);
            Assert.AreEqual(duration, vo.duration);
        }

        [Test]
        public void Constructor_DurationOmitted_DurationIsZero()
        {
            var vo = new TransitionVO(Fade.In);

            Assert.AreEqual(0.0f, vo.duration);
        }

        [Test]
        public void Constructor_DurationIsZero_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new TransitionVO(Fade.In, 0.0f));
        }

        [Test]
        public void Constructor_FadeIsNone_Throws()
        {
            Assert.Catch<Exception>(() => new TransitionVO(Fade.None, 1.0f));
        }

        [Test]
        public void Constructor_DurationIsNegative_Throws()
        {
            Assert.Catch<Exception>(() => new TransitionVO(Fade.In, -0.01f));
        }
    }
}
