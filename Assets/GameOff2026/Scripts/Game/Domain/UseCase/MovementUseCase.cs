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
        private readonly MovementPorts _movementPorts;

        public MovementUseCase(DateEntity dateEntity, FieldEntity fieldEntity, PlayerEntity playerEntity,
            MovementPorts movementPorts)
        {
            _dateEntity = dateEntity;
            _fieldEntity = fieldEntity;
            _playerEntity = playerEntity;
            _movementPorts = movementPorts;
        }

        public IAsyncSubscriber<MovementVO> movement => _movementPorts.movementSubscriber;

        public IAsyncSubscriber<MovableDirectionVO[]> movableDirections => _movementPorts.movableDirectionsSubscriber;

        public UniTask PublishMovableDirectionsAsync(CancellationToken token)
        {
            var directions = PlayerConfig.DIRECTIONS
                .AsValueEnumerable()
                .Select(x =>
                {
                    var nextSquare = _playerEntity.square.AddDirection(x);
                    return new MovableDirectionVO(x, _fieldEntity.IsMovable(nextSquare, _dateEntity.value));
                })
                .ToArray();

            return _movementPorts.PublishMovableDirectionsAsync(directions, token);
        }

        public async UniTask MoveAsync(Direction direction, CancellationToken token)
        {
            var nextSquare = _playerEntity.square.AddDirection(direction);
            _playerEntity.SetSquare(nextSquare);

            var move = new MovementVO(_playerEntity.square, PlayerConfig.MOVE_DURATION);
            await _movementPorts.PublishMovementAsync(move, token);
        }
    }
}