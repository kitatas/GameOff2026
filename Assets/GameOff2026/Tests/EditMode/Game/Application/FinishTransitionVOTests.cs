using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class FinishTransitionVOTests
    {
        [Test]
        public void Constructor_ValidValues_RetainsFinishTypeAndTransition()
        {
            var transition = new TransitionVO(Fade.InOut, 0.25f);

            var finishTransition = new FinishTransitionVO(FinishType.Clear, transition);

            Assert.AreEqual(FinishType.Clear, finishTransition.type);
            Assert.AreSame(transition, finishTransition.transition);
        }

        [Test]
        public void Create_ValidValues_CreatesTransitionWithFadeAndDuration()
        {
            var finishTransition = FinishTransitionVO.Create(FinishType.Fail, Fade.Out, 0.5f);

            Assert.AreEqual(FinishType.Fail, finishTransition.type);
            Assert.AreEqual(Fade.Out, finishTransition.transition.fade);
            Assert.AreEqual(0.5f, finishTransition.transition.duration);
        }

        [Test]
        public void Constructor_NoneType_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() =>
                new FinishTransitionVO(FinishType.None, new TransitionVO(Fade.Out, 0.0f)));
        }
    }
}