using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Utility;
using MessagePipe;
using R3;
using ZLinq;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class MovementUseCase : IDisposable
    {
        private readonly FieldEntity _fieldEntity;
        private readonly PlayerEntity _playerEntity;
        private readonly MovementPorts _movementPorts;
        private readonly Subject<Direction> _direction;

        public MovementUseCase(FieldEntity fieldEntity, PlayerEntity playerEntity, MovementPorts movementPorts)
        {
            _fieldEntity = fieldEntity;
            _playerEntity = playerEntity;
            _movementPorts = movementPorts;
            _direction = new Subject<Direction>();
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
                    return new MovableDirectionVO(x, _fieldEntity.IsMovable(nextSquare));
                })
                .ToArray();

            return _movementPorts.PublishMovableDirectionsAsync(directions, token);
        }

        public void HandleClickDirection(Direction direction)
        {
            // TODO: check input state
            _direction?.OnNext(direction);
        }

        public async UniTask InputAsync(CancellationToken token)
        {
            var direction = await _direction.FirstAsync(cancellationToken: token);
            var nextSquare = _playerEntity.square.AddDirection(direction);
            _playerEntity.SetSquare(nextSquare);
        }

        public async UniTask MoveAsync(CancellationToken token)
        {
            var move = new MovementVO(_playerEntity.square, PlayerConfig.MOVE_DURATION);
            await _movementPorts.PublishMovementAsync(move, token);
        }

        void IDisposable.Dispose()
        {
            _direction?.Dispose();
        }
    }
}