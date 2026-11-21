using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.Ports
{
    [TestFixture]
    public sealed class GameModalPortsTests
    {
        private FakeAsyncSubscriber<BaseModalTransitionVO<GameModalType>> _subscriber;
        private FakeAsyncPublisher<BaseModalTransitionVO<GameModalType>> _publisher;
        private GameModalPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _subscriber = new FakeAsyncSubscriber<BaseModalTransitionVO<GameModalType>>();
            _publisher = new FakeAsyncPublisher<BaseModalTransitionVO<GameModalType>>();
            _ports = new GameModalPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_SubscriberProvided_ExposesSubscriber()
        {
            Assert.AreSame(_subscriber, _ports.subscriber);
        }

        [Test]
        public async Task PublishAsync_ValidTransition_PublishesMessageAndCancellationToken()
        {
            var transition = GameModalTransitionVO.Create(GameModalType.Menu, Fade.In, 0.25f);
            using var source = new CancellationTokenSource();

            await _ports.PublishAsync(transition, source.Token).AsTask();

            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreSame(transition, _publisher.LastPublished);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
        }
    }
}

