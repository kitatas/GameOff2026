using System;
using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class LoadVOTests
    {
        [Test]
        public void Constructor_ValidValues_KeepsValues()
        {
            var transition = new TransitionVO(Fade.In, 0.5f);

            var vo = new LoadVO(SceneName.Game, LoadType.Fade, transition);

            Assert.AreEqual(SceneName.Game, vo.sceneName);
            Assert.AreEqual(LoadType.Fade, vo.loadType);
            Assert.AreSame(transition, vo.transition);
        }

        [Test]
        public void Constructor_TransitionIsNull_DoesNotThrow()
        {
            var vo = new LoadVO(SceneName.Boot, LoadType.Direct, null);

            Assert.IsNull(vo.transition);
        }

        [Test]
        public void Constructor_SceneNameIsNone_Throws()
        {
            Assert.Catch<Exception>(() => new LoadVO(SceneName.None, LoadType.Direct, null));
        }

        [Test]
        public void Constructor_LoadTypeIsNone_Throws()
        {
            Assert.Catch<Exception>(() => new LoadVO(SceneName.Game, LoadType.None, null));
        }

        [Test]
        public void CreateFade_ValidValues_CreatesFadeLoadWithTransition()
        {
            var vo = LoadVO.CreateFade(SceneName.Game, Fade.InOut, 0.25f);

            Assert.AreEqual(SceneName.Game, vo.sceneName);
            Assert.AreEqual(LoadType.Fade, vo.loadType);
            Assert.AreEqual(Fade.InOut, vo.transition.fade);
            Assert.AreEqual(0.25f, vo.transition.duration);
        }

        [Test]
        public void CreateFade_FadeIsNone_Throws()
        {
            Assert.Catch<Exception>(() => LoadVO.CreateFade(SceneName.Game, Fade.None, 0.25f));
        }

        [Test]
        public void CreateFade_DurationIsNegative_Throws()
        {
            Assert.Catch<Exception>(() => LoadVO.CreateFade(SceneName.Game, Fade.In, -1.0f));
        }

        [Test]
        public void CreateFade_SceneNameIsNone_Throws()
        {
            Assert.Catch<Exception>(() => LoadVO.CreateFade(SceneName.None, Fade.In, 0.25f));
        }
    }
}
