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
    public sealed class MovementPortsTests
    {
        [Test]
        public async Task PublishMovementAsync_Movement_PublishesSameValueAndToken()
        {
            var publisher = new FakeAsyncPublisher<MovementVO>();
            var ports = new MovementPorts(publisher, new FakeAsyncSubscriber<MovementVO>());
            var movement = new MovementVO(new SquareVO(2, 4), 0.5f);
            using var source = new CancellationTokenSource();

            await ports.PublishMovementAsync(movement, source.Token).AsTask();

            Assert.AreEqual(1, publisher.PublishCount);
            Assert.AreSame(movement, publisher.LastPublished);
            Assert.AreEqual(source.Token, publisher.tokens[0]);
        }

        [Test]
        public void MovementSubscriber_AfterConstruction_ExposesProvidedSubscriber()
        {
            var subscriber = new FakeAsyncSubscriber<MovementVO>();
            var ports = new MovementPorts(new FakeAsyncPublisher<MovementVO>(), subscriber);

            Assert.AreSame(subscriber, ports.movementSubscriber);
        }
    }
}
