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
    public sealed class HudRootUseCaseTests
    {
        private FakeAsyncSubscriber<HudRootTransitionVO> _subscriber;
        private FakeAsyncPublisher<HudRootTransitionVO> _publisher;
        private HudRootUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _subscriber = new FakeAsyncSubscriber<HudRootTransitionVO>();
            _publisher = new FakeAsyncPublisher<HudRootTransitionVO>();
            var ports = new HudRootPorts(_subscriber, _publisher);
            _useCase = new HudRootUseCase(ports);
        }

        [Test]
        public void Transition_AfterConstruction_ExposesPortsSubscriber()
        {
            Assert.AreSame(_subscriber, _useCase.transition);
        }

        [Test]
        public async Task InitAsync_ValidCall_PublishesOutTransitionWithoutDuration()
        {
            using var source = new CancellationTokenSource();

            await _useCase.InitAsync(source.Token).AsTask();

            var actual = _publisher.LastPublished;
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
            Assert.AreEqual(Fade.Out, actual.transition.fade);
            Assert.AreEqual(0.0f, actual.transition.duration);
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
            Assert.AreEqual(HudRootConfig.FADE_DURATION, actual.transition.duration);
        }
    }
}