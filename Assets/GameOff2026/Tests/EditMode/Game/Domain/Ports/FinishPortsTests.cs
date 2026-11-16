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
    public sealed class FinishPortsTests
    {
        private FakeAsyncSubscriber<FinishTransitionVO> _subscriber;
        private FakeAsyncPublisher<FinishTransitionVO> _publisher;
        private FinishPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _subscriber = new FakeAsyncSubscriber<FinishTransitionVO>();
            _publisher = new FakeAsyncPublisher<FinishTransitionVO>();
            _ports = new FinishPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_SubscriberProvided_ExposesSubscriber()
        {
            Assert.AreSame(_subscriber, _ports.finishTransitionSubscriber);
        }

        [Test]
        public async Task PublishFinishAsync_ValidMessage_PublishesMessageAndCancellationToken()
        {
            var transition = FinishTransitionVO.Create(FinishType.Clear, Fade.Out, 0.25f);
            using var source = new CancellationTokenSource();

            await _ports.PublishFinishAsync(transition, source.Token).AsTask();

            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreSame(transition, _publisher.LastPublished);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
        }
    }
}