using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Data.Entity;
using GameOff2026.Common.Domain.Ports;
using GameOff2026.Common.Domain.UseCase;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Domain.UseCase
{
    [TestFixture]
    public sealed class BasePubSubUseCaseTests
    {
        private TestEntity _entity;
        private FakeAsyncSubscriber<int> _subscriber;
        private FakeAsyncPublisher<int> _publisher;
        private TestUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _entity = new TestEntity();
            _subscriber = new FakeAsyncSubscriber<int>();
            _publisher = new FakeAsyncPublisher<int>();
            _useCase = new TestUseCase(_entity, new TestPorts(_subscriber, _publisher));
        }

        [Test]
        public async Task PublishAsync_Value_UpdatesEntityAndPublishesWithToken()
        {
            using var source = new CancellationTokenSource();

            await _useCase.PublishAsync(42, source.Token).AsTask();

            Assert.AreEqual(42, _entity.value);
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(42, _publisher.LastPublished);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
        }

        [Test]
        public void Subscriber_AfterConstruction_ExposesPortsSubscriber()
        {
            Assert.AreSame(_subscriber, _useCase.subscriber);
        }

        private sealed class TestEntity : BaseEntity<int>
        {
        }

        private sealed class TestPorts : BasePubSubPorts<int>
        {
            public TestPorts(FakeAsyncSubscriber<int> subscriber, FakeAsyncPublisher<int> publisher)
                : base(subscriber, publisher)
            {
            }
        }

        private sealed class TestUseCase : BasePubSubUseCase<int>
        {
            public TestUseCase(TestEntity entity, TestPorts ports) : base(entity, ports)
            {
            }
        }
    }
}
