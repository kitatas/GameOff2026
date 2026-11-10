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
    public sealed class DatePortsTests
    {
        [Test]
        public async Task PublishDateTransitionAsync_Transition_PublishesSameValueAndToken()
        {
            var publisher = new FakeAsyncPublisher<DateTransitionVO>();
            var ports = new DatePorts(new FakeAsyncSubscriber<DateTransitionVO>(), publisher);
            var dateTransition = DateTransitionVO.Create(DateType.Night, Fade.InOut, 0.25f);
            using var source = new CancellationTokenSource();

            await ports.PublishDateTransitionAsync(dateTransition, source.Token).AsTask();

            Assert.AreEqual(1, publisher.PublishCount);
            Assert.AreSame(dateTransition, publisher.LastPublished);
            Assert.AreEqual(source.Token, publisher.tokens[0]);
        }

        [Test]
        public void Constructor_Subscriber_ExposesProvidedSubscriber()
        {
            var subscriber = new FakeAsyncSubscriber<DateTransitionVO>();
            var ports = new DatePorts(subscriber, new FakeAsyncPublisher<DateTransitionVO>());

            Assert.AreSame(subscriber, ports.dateTransitionSubscriber);
        }
    }
}