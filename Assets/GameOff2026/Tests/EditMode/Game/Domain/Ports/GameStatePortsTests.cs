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
    public sealed class GameStatePortsTests
    {
        [Test]
        public async Task PublishAsync_GameState_PublishesSameState()
        {
            var publisher = new FakeAsyncPublisher<GameState>();
            var ports = new GameStatePorts(new FakeAsyncSubscriber<GameState>(), publisher);
            using var source = new CancellationTokenSource();

            await ports.PublishAsync(GameState.Shop, source.Token).AsTask();

            Assert.AreEqual(GameState.Shop, publisher.LastPublished);
            Assert.AreEqual(source.Token, publisher.tokens[0]);
        }

        [Test]
        public void Constructor_Subscriber_ExposesProvidedSubscriber()
        {
            var subscriber = new FakeAsyncSubscriber<GameState>();
            var ports = new GameStatePorts(subscriber, new FakeAsyncPublisher<GameState>());

            Assert.AreSame(subscriber, ports.subscriber);
        }
    }
}

