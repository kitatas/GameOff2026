using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Utility;
using MessagePipe;
using ZLinq;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class MovementUseCase
    {
        private readonly DateEntity _dateEntity;
        private readonly FieldEntity _fieldEntity;
        private readonly PlayerEntity _playerEntity;
        private readonly PurchasedItemEntity _purchasedItemEntity;
        private readonly MovementPorts _movementPorts;

        public MovementUseCase(DateEntity dateEntity, FieldEntity fieldEntity, PlayerEntity playerEntity,
            PurchasedItemEntity purchasedItemEntity, MovementPorts movementPorts)
        {
            _dateEntity = dateEntity;
            _fieldEntity = fieldEntity;
            _playerEntity = playerEntity;
            _purchasedItemEntity = purchasedItemEntity;
            _movementPorts = movementPorts;
        }

        public IAsyncSubscriber<MovementVO> movement => _movementPorts.movementSubscriber;

        public IAsyncSubscriber<MovableDirectionVO[]> movableDirections => _movementPorts.movableDirectionsSubscriber;

        public UniTask SetUpAsync(CancellationToken token)
        {
            _playerEntity.SetSquare(new SquareVO(FieldConfig.MIN_FILE, FieldConfig.MAX_RANK));
            var move = new MovementVO(_playerEntity.square, 0.0f);
            return _movementPorts.PublishMovementAsync(move, token);
        }

        public UniTask PublishMovableDirectionsAsync(CancellationToken token)
        {
            var hasThroughBlock = _purchasedItemEntity.Has(ShopItemId.ThroughBlock);
            var directions = PlayerConfig.DIRECTIONS
                .AsValueEnumerable()
                .Select(x =>
                {
                    var nextSquare = _playerEntity.square.AddDirection(x);
                    var isMovable = _fieldEntity.IsMovable(nextSquare, _dateEntity.value, hasThroughBlock);
                    return new MovableDirectionVO(x, isMovable);
                })
                .ToArray();

            return _movementPorts.PublishMovableDirectionsAsync(directions, token);
        }

        public UniTask MoveAsync(Direction direction, CancellationToken token)
        {
            var nextSquare = _playerEntity.square.AddDirection(direction);
            _playerEntity.SetSquare(nextSquare);

            var move = new MovementVO(_playerEntity.square, PlayerConfig.MOVE_DURATION);
            return _movementPorts.PublishMovementAsync(move, token);
        }
    }
}