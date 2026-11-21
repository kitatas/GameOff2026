using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Domain.Ports;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Domain.Ports
{
    [TestFixture]
    public sealed class BaseModalPortsTests
    {
        private enum TestModalType
        {
            None,
            Menu,
        }

        private FakeAsyncSubscriber<TestModalType> _subscriber;
        private FakeAsyncPublisher<TestModalType> _publisher;
        private TestModalPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _subscriber = new FakeAsyncSubscriber<TestModalType>();
            _publisher = new FakeAsyncPublisher<TestModalType>();
            _ports = new TestModalPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_SubscriberProvided_ExposesSubscriber()
        {
            Assert.AreSame(_subscriber, _ports.subscriber);
        }

        [Test]
        public async Task PublishAsync_ValidMessage_PublishesMessageAndCancellationToken()
        {
            using var source = new CancellationTokenSource();

            await _ports.PublishAsync(TestModalType.Menu, source.Token).AsTask();

            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(TestModalType.Menu, _publisher.LastPublished);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
        }

        private sealed class TestModalPorts : BaseModalPorts<TestModalType>
        {
            public TestModalPorts(FakeAsyncSubscriber<TestModalType> subscriber,
                FakeAsyncPublisher<TestModalType> publisher) : base(subscriber, publisher)
            {
            }
        }
    }
}

