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
    public sealed class HudRootPortsTests
    {
        private FakeAsyncSubscriber<HudRootTransitionVO> _subscriber;
        private FakeAsyncPublisher<HudRootTransitionVO> _publisher;
        private HudRootPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _subscriber = new FakeAsyncSubscriber<HudRootTransitionVO>();
            _publisher = new FakeAsyncPublisher<HudRootTransitionVO>();
            _ports = new HudRootPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_SubscriberProvided_ExposesSubscriber()
        {
            Assert.AreSame(_subscriber, _ports.hudRootTransitionSubscriber);
        }

        [Test]
        public async Task PublishHudRootTransitionAsync_ValidMessage_PublishesMessageAndCancellationToken()
        {
            var transition = HudRootTransitionVO.Create(Fade.Out, 0.25f);
            using var source = new CancellationTokenSource();

            await _ports.PublishHudRootTransitionAsync(transition, source.Token).AsTask();

            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreSame(transition, _publisher.LastPublished);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
        }
    }
}