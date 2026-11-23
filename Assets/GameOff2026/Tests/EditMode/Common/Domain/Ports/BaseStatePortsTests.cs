using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Domain.Ports;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Domain.Ports
{
    [TestFixture]
    public sealed class BaseStatePortsTests
    {
        [Test]
        public async Task PublishAsync_EnumState_PublishesSameState()
        {
            var publisher = new FakeAsyncPublisher<TestState>();
            var ports = new TestStatePorts(new FakeAsyncSubscriber<TestState>(), publisher);
            using var source = new CancellationTokenSource();

            await ports.PublishAsync(TestState.Active, source.Token).AsTask();

            Assert.AreEqual(TestState.Active, publisher.LastPublished);
            Assert.AreEqual(source.Token, publisher.tokens[0]);
        }

        private enum TestState
        {
            Inactive,
            Active,
        }

        private sealed class TestStatePorts : BaseStatePorts<TestState>
        {
            public TestStatePorts(MessagePipe.IAsyncSubscriber<TestState> subscriber, MessagePipe.IAsyncPublisher<TestState> publisher)
                : base(subscriber, publisher)
            {
            }
        }
    }
}

