using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Domain.Ports;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Domain.Ports
{
    [TestFixture]
    public sealed class BasePubSubPortsTests
    {
        [Test]
        public async Task PublishAsync_MessageAndToken_PublishesSameValues()
        {
            var subscriber = new FakeAsyncSubscriber<int>();
            var publisher = new FakeAsyncPublisher<int>();
            var ports = new TestPubSubPorts(subscriber, publisher);
            using var source = new CancellationTokenSource();

            await ports.PublishAsync(12, source.Token).AsTask();

            Assert.AreEqual(1, publisher.PublishCount);
            Assert.AreEqual(12, publisher.LastPublished);
            Assert.AreEqual(source.Token, publisher.tokens[0]);
        }

        [Test]
        public void Constructor_Subscriber_ExposesProvidedSubscriber()
        {
            var subscriber = new FakeAsyncSubscriber<int>();
            var ports = new TestPubSubPorts(subscriber, new FakeAsyncPublisher<int>());

            Assert.AreSame(subscriber, ports.subscriber);
        }

        private sealed class TestPubSubPorts : BasePubSubPorts<int>
        {
            public TestPubSubPorts(MessagePipe.IAsyncSubscriber<int> subscriber, MessagePipe.IAsyncPublisher<int> publisher)
                : base(subscriber, publisher)
            {
            }
        }
    }
}

