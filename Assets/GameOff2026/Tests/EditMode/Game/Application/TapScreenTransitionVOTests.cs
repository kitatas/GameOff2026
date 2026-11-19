using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class TapScreenTransitionVOTests
    {
        [Test]
        public void Constructor_ValidTransition_RetainsTransition()
        {
            var transition = new TransitionVO(Fade.InOut, 0.25f);

            var tapScreenTransition = new TapScreenTransitionVO(transition);

            Assert.AreSame(transition, tapScreenTransition.transition);
        }

        [Test]
        public void Create_ValidValues_CreatesTransitionWithFadeAndDuration()
        {
            var tapScreenTransition = TapScreenTransitionVO.Create(Fade.Out, 0.5f);

            Assert.AreEqual(Fade.Out, tapScreenTransition.transition.fade);
            Assert.AreEqual(0.5f, tapScreenTransition.transition.duration);
        }
    }
}