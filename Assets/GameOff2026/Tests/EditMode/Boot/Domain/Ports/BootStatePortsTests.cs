using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Boot.Application;
using GameOff2026.Boot.Domain.Ports;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Boot.Domain.Ports
{
    [TestFixture]
    public sealed class BootStatePortsTests
    {
        [Test]
        public async Task PublishAsync_BootState_PublishesSameState()
        {
            var publisher = new FakeAsyncPublisher<BootState>();
            var ports = new BootStatePorts(new FakeAsyncSubscriber<BootState>(), publisher);
            using var source = new CancellationTokenSource();

            await ports.PublishAsync(BootState.Splash, source.Token).AsTask();

            Assert.AreEqual(BootState.Splash, publisher.LastPublished);
            Assert.AreEqual(source.Token, publisher.tokens[0]);
        }

        [Test]
        public void Constructor_Subscriber_ExposesProvidedSubscriber()
        {
            var subscriber = new FakeAsyncSubscriber<BootState>();
            var ports = new BootStatePorts(subscriber, new FakeAsyncPublisher<BootState>());

            Assert.AreSame(subscriber, ports.subscriber);
        }
    }
}

