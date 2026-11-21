using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class GameModalUseCaseTests
    {
        private GameModalEntity _entity;
        private FakeAsyncSubscriber<BaseModalTransitionVO<GameModalType>> _subscriber;
        private FakeAsyncPublisher<BaseModalTransitionVO<GameModalType>> _publisher;
        private GameModalUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _entity = new GameModalEntity();
            _subscriber = new FakeAsyncSubscriber<BaseModalTransitionVO<GameModalType>>();
            _publisher = new FakeAsyncPublisher<BaseModalTransitionVO<GameModalType>>();
            var ports = new GameModalPorts(_subscriber, _publisher);
            _useCase = new GameModalUseCase(_entity, ports);
        }

        [Test]
        public void Subscriber_AfterConstruction_ExposesPortsSubscriber()
        {
            Assert.AreSame(_subscriber, _useCase.subscriber);
        }

        [Test]
        public async Task FadeAsync_ValidModal_UpdatesEntityAndPublishesConfiguredTransition()
        {
            var modal = new GameModalVO(GameModalType.Menu, Fade.Out);
            using var source = new CancellationTokenSource();

            await _useCase.FadeAsync(modal, source.Token).AsTask();

            var actual = _entity.value as GameModalTransitionVO;
            Assert.IsNotNull(actual);
            Assert.AreEqual(GameModalType.Menu, actual.type);
            Assert.AreEqual(Fade.Out, actual.transition.fade);
            Assert.AreEqual(GameModalConfig.FADE_DURATION, actual.transition.duration);
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreSame(actual, _publisher.LastPublished);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
        }
    }
}

