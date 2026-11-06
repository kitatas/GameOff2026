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
            var movementPublisher = new FakeAsyncPublisher<MovementVO>();
            var ports = new MovementPorts(
                new FakeAsyncSubscriber<MovementVO>(),
                new FakeAsyncSubscriber<MovableDirectionVO[]>(),
                movementPublisher,
                new FakeAsyncPublisher<MovableDirectionVO[]>());
            var movement = new MovementVO(new SquareVO(2, 4), 0.5f);
            using var source = new CancellationTokenSource();

            await ports.PublishMovementAsync(movement, source.Token).AsTask();

            Assert.AreEqual(1, movementPublisher.PublishCount);
            Assert.AreSame(movement, movementPublisher.LastPublished);
            Assert.AreEqual(source.Token, movementPublisher.tokens[0]);
        }

        [Test]
        public async Task PublishMovableDirectionsAsync_Directions_PublishesSameValueAndToken()
        {
            var movableDirectionsPublisher = new FakeAsyncPublisher<MovableDirectionVO[]>();
            var ports = new MovementPorts(
                new FakeAsyncSubscriber<MovementVO>(),
                new FakeAsyncSubscriber<MovableDirectionVO[]>(),
                new FakeAsyncPublisher<MovementVO>(),
                movableDirectionsPublisher);
            var directions = new[]
            {
                new MovableDirectionVO(Direction.Up, true),
                new MovableDirectionVO(Direction.Left, false),
            };
            using var source = new CancellationTokenSource();

            await ports.PublishMovableDirectionsAsync(directions, source.Token).AsTask();

            Assert.AreEqual(1, movableDirectionsPublisher.PublishCount);
            Assert.AreSame(directions, movableDirectionsPublisher.LastPublished);
            Assert.AreEqual(source.Token, movableDirectionsPublisher.tokens[0]);
        }

        [Test]
        public void Subscribers_AfterConstruction_ExposeProvidedSubscribers()
        {
            var movementSubscriber = new FakeAsyncSubscriber<MovementVO>();
            var movableDirectionsSubscriber = new FakeAsyncSubscriber<MovableDirectionVO[]>();
            var ports = new MovementPorts(
                movementSubscriber,
                movableDirectionsSubscriber,
                new FakeAsyncPublisher<MovementVO>(),
                new FakeAsyncPublisher<MovableDirectionVO[]>());

            Assert.AreSame(movementSubscriber, ports.movementSubscriber);
            Assert.AreSame(movableDirectionsSubscriber, ports.movableDirectionsSubscriber);
        }
    }
}
