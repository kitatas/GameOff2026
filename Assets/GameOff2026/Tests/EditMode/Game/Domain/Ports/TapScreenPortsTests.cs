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
    public sealed class TapScreenPortsTests
    {
        private FakeAsyncSubscriber<TapScreenTransitionVO> _subscriber;
        private FakeAsyncPublisher<TapScreenTransitionVO> _publisher;
        private TapScreenPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _subscriber = new FakeAsyncSubscriber<TapScreenTransitionVO>();
            _publisher = new FakeAsyncPublisher<TapScreenTransitionVO>();
            _ports = new TapScreenPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_SubscriberProvided_ExposesSubscriber()
        {
            Assert.AreSame(_subscriber, _ports.tapScreenTransitionSubscriber);
        }

        [Test]
        public async Task PublishTapScreenAsync_ValidMessage_PublishesMessageAndCancellationToken()
        {
            var transition = TapScreenTransitionVO.Create(Fade.Out, 0.25f);
            using var source = new CancellationTokenSource();

            await _ports.PublishTapScreenAsync(transition, source.Token).AsTask();

            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreSame(transition, _publisher.LastPublished);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
        }
    }
}