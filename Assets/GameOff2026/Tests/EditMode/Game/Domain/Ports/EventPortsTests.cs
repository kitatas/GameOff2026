using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.Ports
{
    [TestFixture]
    public sealed class EventPortsTests
    {
        [Test]
        public async Task PublishEventResultAsync_Result_PublishesSameValueAndToken()
        {
            var publisher = new FakeAsyncPublisher<EventResultVO>();
            var ports = new EventPorts(new FakeAsyncSubscriber<EventResultVO>(), publisher);
            var result = new EventResultVO(FieldType.Treasure, 2);
            using var source = new CancellationTokenSource();

            await ports.PublishEventResultAsync(result, source.Token).AsTask();

            Assert.AreEqual(1, publisher.PublishCount);
            Assert.AreSame(result, publisher.LastPublished);
            Assert.AreEqual(source.Token, publisher.tokens[0]);
        }

        [Test]
        public void EventResultSubscriber_AfterConstruction_ExposesProvidedSubscriber()
        {
            var subscriber = new FakeAsyncSubscriber<EventResultVO>();
            var ports = new EventPorts(subscriber, new FakeAsyncPublisher<EventResultVO>());

            Assert.AreSame(subscriber, ports.eventResultSubscriber);
        }
    }
}
