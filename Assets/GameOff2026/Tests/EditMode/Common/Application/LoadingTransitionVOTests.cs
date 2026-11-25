using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class LoadingTransitionVOTests
    {
        [Test]
        public void Create_ValidFade_KeepsCreatedTransition()
        {
            var vo = LoadingTransitionVO.Create(Fade.Out, 0.25f);

            Assert.AreEqual(Fade.Out, vo.transition.fade);
            Assert.AreEqual(0.25f, vo.transition.duration);
        }

        [Test]
        public void Create_InvalidFade_Throws()
        {
            Assert.Catch<System.Exception>(() => LoadingTransitionVO.Create(Fade.None, 0.25f));
        }

        [Test]
        public void Create_NegativeDuration_Throws()
        {
            Assert.Catch<System.Exception>(() => LoadingTransitionVO.Create(Fade.In, -0.01f));
        }
    }
}

