using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
        private DateEntity _dateEntity;
        private PlayerEntity _playerEntity;
        private PurchasedItemEntity _purchasedItemEntity;
        private FakeAsyncPublisher<MovementVO> _movementPublisher;
        private FakeAsyncPublisher<MovableDirectionVO[]> _movableDirectionsPublisher;
        private FakeAsyncSubscriber<MovementVO> _movementSubscriber;
        private FakeAsyncSubscriber<MovableDirectionVO[]> _movableDirectionsSubscriber;
        private MovementUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _fieldEntity = new FieldEntity();
            _dateEntity = new DateEntity();
            _dateEntity.Reset();
            _playerEntity = new PlayerEntity();
            _purchasedItemEntity = new PurchasedItemEntity();
            _movementPublisher = new FakeAsyncPublisher<MovementVO>();
            _movableDirectionsPublisher = new FakeAsyncPublisher<MovableDirectionVO[]>();
            _movementSubscriber = new FakeAsyncSubscriber<MovementVO>();
            _movableDirectionsSubscriber = new FakeAsyncSubscriber<MovableDirectionVO[]>();
            var ports = new MovementPorts(
                _movementSubscriber,
                _movableDirectionsSubscriber,
                _movementPublisher,
                _movableDirectionsPublisher);
            _useCase = new MovementUseCase(_dateEntity, _fieldEntity, _playerEntity, _purchasedItemEntity, ports);
        }

        [TestCase(Direction.Up, 2, 4)]
        [TestCase(Direction.Down, 2, 2)]
        [TestCase(Direction.Left, 1, 3)]
        [TestCase(Direction.Right, 3, 3)]
        public async Task MoveAsync_Direction_UpdatesPlayerSquareAndPublishesMovement(
            Direction direction, int expectedFile, int expectedRank)
        {
            _playerEntity.SetSquare(new SquareVO(2, 3));
            using var source = new CancellationTokenSource();

            await _useCase.MoveAsync(direction, source.Token).AsTask();

            Assert.AreEqual(expectedFile, _playerEntity.square.file);
            Assert.AreEqual(expectedRank, _playerEntity.square.rank);
            Assert.AreEqual(1, _movementPublisher.PublishCount);
            Assert.AreEqual(expectedFile, _movementPublisher.LastPublished.square.file);
            Assert.AreEqual(expectedRank, _movementPublisher.LastPublished.square.rank);
            Assert.AreSame(_playerEntity.square, _movementPublisher.LastPublished.square);
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
            _fieldEntity.Add(FieldSquareVO.Create(1, 2, new FieldVO(FieldType.Empty, TestSpriteFactory.Valid, 1.0f)));
            _fieldEntity.Add(FieldSquareVO.Create(2, 1, new FieldVO(FieldType.Empty, TestSpriteFactory.Valid, 1.0f)));

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

        [Test]
        public async Task SetUpAsync_AfterCurrentPlayerSquare_ResetsSquareAndPublishesMovement()
        {
            _playerEntity.SetSquare(new SquareVO(3, 2));
            using var source = new CancellationTokenSource();

            await _useCase.SetUpAsync(source.Token).AsTask();

            Assert.AreEqual(FieldConfig.MIN_FILE, _playerEntity.square.file);
            Assert.AreEqual(FieldConfig.MAX_RANK, _playerEntity.square.rank);
            Assert.AreEqual(1, _movementPublisher.PublishCount);
            Assert.AreEqual(FieldConfig.MIN_FILE, _movementPublisher.LastPublished.square.file);
            Assert.AreEqual(FieldConfig.MAX_RANK, _movementPublisher.LastPublished.square.rank);
            Assert.AreSame(_playerEntity.square, _movementPublisher.LastPublished.square);
            Assert.AreEqual(0.0f, _movementPublisher.LastPublished.duration);
            Assert.AreEqual(source.Token, _movementPublisher.tokens[0]);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task PublishMovableDirectionsAsync_BlockAndThroughBlockItem_ReportsExpectedMovability(
            bool hasThroughBlock)
        {
            _playerEntity.SetSquare(new SquareVO(1, 1));
            _fieldEntity.Add(FieldSquareVO.Create(1, 2,
                new FieldVO(FieldType.Block, TestSpriteFactory.Valid, 1.0f)));
            if (hasThroughBlock)
            {
                _purchasedItemEntity.Add((int)ShopItemId.ThroughBlock);
            }

            await _useCase.PublishMovableDirectionsAsync(CancellationToken.None).AsTask();

            var upDirection = System.Array.Find(_movableDirectionsPublisher.LastPublished,
                x => x.direction == Direction.Up);
            Assert.AreEqual(hasThroughBlock, upDirection.isMovable);
        }
    }
}