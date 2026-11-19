using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class HudRootTransitionVOTests
    {
        [Test]
        public void Constructor_ValidTransition_RetainsTransition()
        {
            var transition = new TransitionVO(Fade.InOut, 0.25f);

            var hudRootTransition = new HudRootTransitionVO(transition);

            Assert.AreSame(transition, hudRootTransition.transition);
        }

        [Test]
        public void Create_ValidValues_CreatesTransitionWithFadeAndDuration()
        {
            var hudRootTransition = HudRootTransitionVO.Create(Fade.Out, 0.5f);

            Assert.AreEqual(Fade.Out, hudRootTransition.transition.fade);
            Assert.AreEqual(0.5f, hudRootTransition.transition.duration);
        }
    }
}