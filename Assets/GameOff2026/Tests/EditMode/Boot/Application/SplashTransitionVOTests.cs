using GameOff2026.Boot.Application;
using GameOff2026.Common.Application;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Boot.Application
{
    [TestFixture]
    public sealed class SplashTransitionVOTests
    {
        [Test]
        public void Constructor_ValidValues_RetainsSplashAndTransition()
        {
            var splash = new SplashVO(SplashType.Developer, TestSpriteFactory.Valid);
            var transition = new TransitionVO(Fade.InOut, 0.5f);

            var splashTransition = new SplashTransitionVO(splash, transition);

            Assert.AreSame(splash, splashTransition.splash);
            Assert.AreSame(transition, splashTransition.transition);
        }

        [Test]
        public void Create_NullSplashFadeOutAndZeroDuration_CreatesFadeOutTransition()
        {
            var splashTransition = SplashTransitionVO.Create(null, Fade.Out, 0.0f);

            Assert.IsNull(splashTransition.splash);
            Assert.AreEqual(Fade.Out, splashTransition.transition.fade);
            Assert.AreEqual(0.0f, splashTransition.transition.duration);
        }
    }
}

