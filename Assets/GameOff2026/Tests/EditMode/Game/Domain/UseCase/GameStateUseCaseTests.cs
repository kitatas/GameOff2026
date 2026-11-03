using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Data.Entity;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class GameStateUseCaseTests
    {
        private GameStateEntity _entity;
        private FakeAsyncPublisher<GameState> _publisher;
        private GameStateUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _entity = new GameStateEntity();
            _publisher = new FakeAsyncPublisher<GameState>();
            var ports = new GameStatePorts(new FakeAsyncSubscriber<GameState>(), _publisher);
            _useCase = new GameStateUseCase(_entity, new RetryCountEntity(), ports);
        }

        [TearDown]
        public void TearDown()
        {
            _useCase.Dispose();
        }

        [Test]
        public async Task InitAsync_InitializesAndPublishesInitState()
        {
            using var source = new CancellationTokenSource();

            await _useCase.InitAsync(source.Token).AsTask();

            Assert.AreEqual(GameState.Init, _entity.value);
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(GameState.Init, _publisher.LastPublished);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
        }

        [Test]
        public void PublishAsync_NoneWithCanceledToken_DoesNotPublishOrChangeState()
        {
            using var source = new CancellationTokenSource();
            source.Cancel();

            Assert.CatchAsync<System.OperationCanceledException>(
                async () => await _useCase.PublishAsync(GameState.None, source.Token).AsTask());

            Assert.AreEqual(0, _publisher.PublishCount);
            Assert.AreEqual(default(GameState), _entity.value);
        }

        [Test]
        public async Task PublishAsync_ValidState_UpdatesEntityAndPublishes()
        {
            await _useCase.PublishAsync(GameState.Input, CancellationToken.None).AsTask();

            Assert.AreEqual(GameState.Input, _entity.value);
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(GameState.Input, _publisher.LastPublished);
        }
    }
}
