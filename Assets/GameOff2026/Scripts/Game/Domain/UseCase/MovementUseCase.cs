using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using MessagePipe;
using R3;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class MovementUseCase : IDisposable
    {
        private readonly PlayerEntity _playerEntity;
        private readonly MovementPorts _movementPorts;
        private readonly Subject<Direction> _direction;

        public MovementUseCase(PlayerEntity playerEntity, MovementPorts movementPorts)
        {
            _playerEntity = playerEntity;
            _movementPorts = movementPorts;
            _direction = new Subject<Direction>();
        }

        public IAsyncSubscriber<MovementVO> movement => _movementPorts.movementSubscriber;

        public void HandleClickDirection(Direction direction)
        {
            // TODO: check input state
            _direction?.OnNext(direction);
        }

        public async UniTask InputAsync(CancellationToken token)
        {
            var direction = await _direction.FirstAsync(cancellationToken: token);

            var dir = direction.ToVector2();
            var file = _playerEntity.square.file + dir.x;
            var rank = _playerEntity.square.rank + dir.y;
            _playerEntity.SetSquare(new SquareVO(file, rank));
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