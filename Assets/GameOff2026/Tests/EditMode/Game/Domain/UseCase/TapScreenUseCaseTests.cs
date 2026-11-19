using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class TapScreenUseCaseTests
    {
        private FakeAsyncSubscriber<TapScreenTransitionVO> _subscriber;
        private FakeAsyncPublisher<TapScreenTransitionVO> _publisher;
        private TapScreenUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _subscriber = new FakeAsyncSubscriber<TapScreenTransitionVO>();
            _publisher = new FakeAsyncPublisher<TapScreenTransitionVO>();
            var ports = new TapScreenPorts(_subscriber, _publisher);
            _useCase = new TapScreenUseCase(ports);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void TapScreenTransition_AfterConstruction_ExposesPortsSubscriber()
        {
            Assert.AreSame(_subscriber, _useCase.tapScreenTransition);
        }

        [Test]
        public async Task FadeAsync_ValidValues_PublishesTransitionWithConfiguredDuration()
        {
            using var source = new CancellationTokenSource();

            await _useCase.FadeAsync(Fade.In, source.Token).AsTask();

            var actual = _publisher.LastPublished;
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
            Assert.AreEqual(Fade.In, actual.transition.fade);
            Assert.AreEqual(TapScreenConfig.FADE_DURATION, actual.transition.duration);
        }

        [Test]
        public async Task WaitForTapScreenAsync_NotifyCalled_CompletesWait()
        {
            using var source = new CancellationTokenSource();
            var waitTask = _useCase.WaitForTapScreenAsync(source.Token).AsTask();

            _useCase.NotifyTapScreen();

            await waitTask;
        }
    }
}