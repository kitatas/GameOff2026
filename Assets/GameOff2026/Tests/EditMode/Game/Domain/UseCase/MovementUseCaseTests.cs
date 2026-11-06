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
        private FieldEntity _fieldEntity;
        private PlayerEntity _playerEntity;
        private FakeAsyncPublisher<MovementVO> _movementPublisher;
        private FakeAsyncPublisher<MovableDirectionVO[]> _movableDirectionsPublisher;
        private FakeAsyncSubscriber<MovementVO> _movementSubscriber;
        private FakeAsyncSubscriber<MovableDirectionVO[]> _movableDirectionsSubscriber;
        private MovementUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _fieldEntity = new FieldEntity();
            _playerEntity = new PlayerEntity();
            _movementPublisher = new FakeAsyncPublisher<MovementVO>();
            _movableDirectionsPublisher = new FakeAsyncPublisher<MovableDirectionVO[]>();
            _movementSubscriber = new FakeAsyncSubscriber<MovementVO>();
            _movableDirectionsSubscriber = new FakeAsyncSubscriber<MovableDirectionVO[]>();
            var ports = new MovementPorts(
                _movementSubscriber,
                _movableDirectionsSubscriber,
                _movementPublisher,
                _movableDirectionsPublisher);
            _useCase = new MovementUseCase(_fieldEntity, _playerEntity, ports);
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

            Assert.AreEqual(1, _playerEntity.square.file);
            Assert.AreEqual(6, _playerEntity.square.rank);
        }

        [Test]
        public async Task MoveAsync_CurrentSquare_PublishesMovementWithConfiguredDurationAndToken()
        {
            var square = new SquareVO(2, 3);
            _playerEntity.SetSquare(square);
            using var source = new CancellationTokenSource();

            await _useCase.MoveAsync(source.Token).AsTask();

            Assert.AreEqual(1, _movementPublisher.PublishCount);
            Assert.AreSame(square, _movementPublisher.LastPublished.square);
            Assert.AreEqual(PlayerConfig.MOVE_DURATION, _movementPublisher.LastPublished.duration);
            Assert.AreEqual(source.Token, _movementPublisher.tokens[0]);
        }

        [Test]
        public async Task PublishMovableDirectionsAsync_Directions_PublishesAllAsNotMovable()
        {
            using var source = new CancellationTokenSource();

            await _useCase.PublishMovableDirectionsAsync(source.Token).AsTask();

            var directions = _movableDirectionsPublisher.LastPublished;
            Assert.AreEqual(PlayerConfig.DIRECTIONS.Length, directions.Length);
            Assert.AreEqual(1, _movableDirectionsPublisher.PublishCount);
            Assert.AreEqual(source.Token, _movableDirectionsPublisher.tokens[0]);

            for (int i = 0; i < PlayerConfig.DIRECTIONS.Length; i++)
            {
                Assert.AreEqual(PlayerConfig.DIRECTIONS[i], directions[i].direction);
                Assert.IsFalse(directions[i].isMovable);
            }
        }

        [Test]
        public async Task PublishMovableDirectionsAsync_AdjacentSquaresExist_ReportsMovableDirections()
        {
            _playerEntity.SetSquare(new SquareVO(1, 1));
            _fieldEntity.Add(FieldSquareVO.Create(1, 2, new FieldVO(FieldType.Ground, null, 1.0f)));
            _fieldEntity.Add(FieldSquareVO.Create(2, 1, new FieldVO(FieldType.Ground, null, 1.0f)));

            await _useCase.PublishMovableDirectionsAsync(CancellationToken.None).AsTask();

            var directions = _movableDirectionsPublisher.LastPublished;
            Assert.AreEqual(PlayerConfig.DIRECTIONS.Length, directions.Length);
            Assert.AreEqual(Direction.Up, directions[0].direction);
            Assert.IsTrue(directions[0].isMovable);
            Assert.AreEqual(Direction.Down, directions[1].direction);
            Assert.IsFalse(directions[1].isMovable);
            Assert.AreEqual(Direction.Left, directions[2].direction);
            Assert.IsFalse(directions[2].isMovable);
            Assert.AreEqual(Direction.Right, directions[3].direction);
            Assert.IsTrue(directions[3].isMovable);
        }

        [Test]
        public void Subscribers_AfterConstruction_ExposePortsSubscribers()
        {
            Assert.AreSame(_movementSubscriber, _useCase.movement);
            Assert.AreSame(_movableDirectionsSubscriber, _useCase.movableDirections);
        }
    }
}
