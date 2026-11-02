using System;
using System.Collections.Generic;
using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.UseCase;
using NUnit.Framework;
using R3;

namespace GameOff2026.Tests.EditMode.Common.Domain.UseCase
{
    [TestFixture]
    public sealed class SceneUseCaseTests
    {
        private SceneUseCase _useCase;
        private List<LoadVO> _received;
        private IDisposable _subscription;

        [SetUp]
        public void SetUp()
        {
            _useCase = new SceneUseCase();
            _received = new List<LoadVO>();
            _subscription = _useCase.load.Subscribe(vo => _received.Add(vo));
        }

        [TearDown]
        public void TearDown()
        {
            _subscription.Dispose();
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void LoadFade_ValidArguments_PublishesFadeLoadVO()
        {
            _useCase.LoadFade(SceneName.Game, Fade.InOut);

            Assert.AreEqual(1, _received.Count);
            Assert.AreEqual(SceneName.Game, _received[0].sceneName);
            Assert.AreEqual(LoadType.Fade, _received[0].loadType);
            Assert.AreEqual(Fade.InOut, _received[0].transition.fade);
        }

        [Test]
        public void LoadFade_ValidArguments_UsesConfiguredFadeDuration()
        {
            _useCase.LoadFade(SceneName.Boot, Fade.In);

            Assert.AreEqual(SceneConfig.FADE_DURATION, _received[0].transition.duration);
        }

        [Test]
        public void LoadFade_CalledTwice_PublishesInOrder()
        {
            _useCase.LoadFade(SceneName.Boot, Fade.In);
            _useCase.LoadFade(SceneName.Game, Fade.Out);

            Assert.AreEqual(2, _received.Count);
            Assert.AreEqual(SceneName.Boot, _received[0].sceneName);
            Assert.AreEqual(SceneName.Game, _received[1].sceneName);
        }

        [Test]
        public void LoadFade_NoSubscriber_DoesNotThrow()
        {
            var useCase = new SceneUseCase();

            Assert.DoesNotThrow(() => useCase.LoadFade(SceneName.Game, Fade.In));

            ((IDisposable)useCase).Dispose();
        }

        [Test]
        public void LoadFade_SceneNameIsNone_ThrowsAndPublishesNothing()
        {
            Assert.Catch<Exception>(() => _useCase.LoadFade(SceneName.None, Fade.In));

            Assert.AreEqual(0, _received.Count);
        }

        [Test]
        public void LoadFade_FadeIsNone_ThrowsAndPublishesNothing()
        {
            Assert.Catch<Exception>(() => _useCase.LoadFade(SceneName.Game, Fade.None));

            Assert.AreEqual(0, _received.Count);
        }

        [Test]
        public void Dispose_Subscribed_CompletesLoadStream()
        {
            var completed = false;
            using var completionSubscription = _useCase.load.Subscribe(_ => { }, _ => completed = true);

            ((IDisposable)_useCase).Dispose();

            Assert.IsTrue(completed);
        }
    }
}
