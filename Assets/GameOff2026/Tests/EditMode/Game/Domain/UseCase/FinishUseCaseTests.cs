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
    public sealed class FinishUseCaseTests
    {
        private FakeAsyncSubscriber<FinishTransitionVO> _subscriber;
        private FakeAsyncPublisher<FinishTransitionVO> _publisher;
        private FinishUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _subscriber = new FakeAsyncSubscriber<FinishTransitionVO>();
            _publisher = new FakeAsyncPublisher<FinishTransitionVO>();
            var ports = new FinishPorts(_subscriber, _publisher);
            _useCase = new FinishUseCase(ports);
        }

        [Test]
        public void FinishTransition_AfterConstruction_ExposesPortsSubscriber()
        {
            Assert.AreSame(_subscriber, _useCase.finishTransition);
        }

        [Test]
        public async Task InitAsync_ValidFinishType_PublishesOutTransitionWithoutDuration()
        {
            using var source = new CancellationTokenSource();

            await _useCase.InitAsync(FinishType.Clear, source.Token).AsTask();

            var actual = _publisher.LastPublished;
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
            Assert.AreEqual(FinishType.Clear, actual.type);
            Assert.AreEqual(Fade.Out, actual.transition.fade);
            Assert.AreEqual(0.0f, actual.transition.duration);
        }

        [Test]
        public async Task FadeAsync_ValidValues_PublishesTransitionWithConfiguredDuration()
        {
            using var source = new CancellationTokenSource();

            await _useCase.FadeAsync(FinishType.Fail, Fade.In, source.Token).AsTask();

            var actual = _publisher.LastPublished;
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
            Assert.AreEqual(FinishType.Fail, actual.type);
            Assert.AreEqual(Fade.In, actual.transition.fade);
            Assert.AreEqual(FinishConfig.FADE_DURATION, actual.transition.duration);
        }
    }
}