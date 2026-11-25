using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.Ports;
using GameOff2026.Common.Domain.UseCase;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Domain.UseCase
{
    [TestFixture]
    public sealed class LoadingUseCaseTests
    {
        private LoadingUseCase _useCase;
        private FakeAsyncSubscriber<LoadingTransitionVO> _subscriber;
        private FakeAsyncPublisher<LoadingTransitionVO> _publisher;

        [SetUp]
        public void SetUp()
        {
            _subscriber = new FakeAsyncSubscriber<LoadingTransitionVO>();
            _publisher = new FakeAsyncPublisher<LoadingTransitionVO>();
            var ports = new LoadingPorts(_subscriber, _publisher);
            _useCase = new LoadingUseCase(ports);
        }

        [Test]
        public void Transition_AfterConstruction_ExposesPortsSubscriber()
        {
            Assert.AreSame(_subscriber, _useCase.transition);
        }

        [Test]
        public async Task InitAsync_Always_PublishesFadeOutWithoutDuration()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(Fade.Out, _publisher.LastPublished.transition.fade);
            Assert.AreEqual(0.0f, _publisher.LastPublished.transition.duration);
        }

        [Test]
        public async Task FadeAsync_ValidFade_PublishesConfiguredDuration()
        {
            await _useCase.FadeAsync(Fade.In, CancellationToken.None).AsTask();

            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(Fade.In, _publisher.LastPublished.transition.fade);
            Assert.AreEqual(LoadingConfig.FADE_DURATION, _publisher.LastPublished.transition.duration);
        }

        [Test]
        public void FadeAsync_InvalidFade_ThrowsAndPublishesNothing()
        {
            Assert.Catch<Exception>(() => _useCase.FadeAsync(Fade.None, CancellationToken.None));

            Assert.AreEqual(0, _publisher.PublishCount);
        }
    }
}