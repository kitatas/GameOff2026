using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class GameModalTransitionVOTests
    {
        [Test]
        public void Constructor_ValidValues_KeepsTypeAndTransition()
        {
            var transition = new TransitionVO(Fade.Out, 0.5f);

            var vo = new GameModalTransitionVO(GameModalType.Menu, transition);

            Assert.AreEqual(GameModalType.Menu, vo.type);
            Assert.AreSame(transition, vo.transition);
        }

        [Test]
        public void Constructor_TypeIsNone_ThrowsQuitException()
        {
            var transition = new TransitionVO(Fade.In);

            Assert.Throws<QuitExceptionVO>(() =>
                new GameModalTransitionVO(GameModalType.None, transition));
        }

        [Test]
        public void Create_WithValues_CreatesTransitionWithConfiguredFadeAndDuration()
        {
            var vo = GameModalTransitionVO.Create(GameModalType.Menu, Fade.InOut, 0.75f);

            Assert.AreEqual(GameModalType.Menu, vo.type);
            Assert.AreEqual(Fade.InOut, vo.transition.fade);
            Assert.AreEqual(0.75f, vo.transition.duration);
        }

        [Test]
        public void Create_WithModal_CopiesTypeAndFadeAndUsesDuration()
        {
            var modal = new GameModalVO(GameModalType.Menu, Fade.Out);

            var vo = GameModalTransitionVO.Create(modal, 0.4f);

            Assert.AreEqual(GameModalType.Menu, vo.type);
            Assert.AreEqual(Fade.Out, vo.transition.fade);
            Assert.AreEqual(0.4f, vo.transition.duration);
        }

        [Test]
        public void Create_WithNegativeDuration_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() =>
                GameModalTransitionVO.Create(GameModalType.Menu, Fade.In, -0.01f));
        }
    }
}
