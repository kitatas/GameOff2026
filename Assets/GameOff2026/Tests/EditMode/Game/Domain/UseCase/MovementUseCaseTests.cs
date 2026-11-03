using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class MovementUseCaseTests
    {
        private PlayerEntity _playerEntity;
        private FakeAsyncPublisher<MovementVO> _publisher;
        private FakeAsyncSubscriber<MovementVO> _subscriber;
        private MovementUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _playerEntity = new PlayerEntity();
            _publisher = new FakeAsyncPublisher<MovementVO>();
            _subscriber = new FakeAsyncSubscriber<MovementVO>();
            _useCase = new MovementUseCase(_playerEntity, new MovementPorts(_publisher, _subscriber));
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [TestCase(Direction.Up, 2, 4)]
        [TestCase(Direction.Down, 2, 2)]
        [TestCase(Direction.Left, 1, 3)]
        [TestCase(Direction.Right, 3, 3)]
        public async Task InputAsync_Direction_UpdatesPlayerSquare(Direction direction, int expectedFile,
            int expectedRank)
        {
            _playerEntity.SetSquare(new SquareVO(2, 3));
            var inputTask = _useCase.InputAsync(CancellationToken.None).AsTask();

            _useCase.HandleClickDirection(direction);
            await inputTask;

            Assert.AreEqual(expectedFile, _playerEntity.square.file);
            Assert.AreEqual(expectedRank, _playerEntity.square.rank);
        }

        [Test]
        public void InputAsync_NoneDirection_ThrowsWithoutChangingSquare()
        {
            var inputTask = _useCase.InputAsync(CancellationToken.None).AsTask();
            _useCase.HandleClickDirection(Direction.None);

            Assert.ThrowsAsync<QuitExceptionVO>(async () => await inputTask);

            Assert.AreEqual(0, _playerEntity.square.file);
            Assert.AreEqual(0, _playerEntity.square.rank);
        }

        [Test]
        public async Task MoveAsync_CurrentSquare_PublishesMovementWithConfiguredDurationAndToken()
        {
            var square = new SquareVO(2, 3);
            _playerEntity.SetSquare(square);
            using var source = new CancellationTokenSource();

            await _useCase.MoveAsync(source.Token).AsTask();

            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreSame(square, _publisher.LastPublished.square);
            Assert.AreEqual(PlayerConfig.MOVE_DURATION, _publisher.LastPublished.duration);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
        }

        [Test]
        public void Movement_AfterConstruction_ExposesPortsSubscriber()
        {
            Assert.AreSame(_subscriber, _useCase.movement);
        }
    }
}
