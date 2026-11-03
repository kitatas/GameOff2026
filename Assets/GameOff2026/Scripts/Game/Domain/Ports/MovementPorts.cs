using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using MessagePipe;

namespace GameOff2026.Game.Domain.Ports
{
    public sealed class MovementPorts
    {
        public readonly IAsyncSubscriber<MovementVO> movementSubscriber;
        private readonly IAsyncPublisher<MovementVO> _movementPublisher;

        public MovementPorts(IAsyncPublisher<MovementVO> movementPublisher,
            IAsyncSubscriber<MovementVO> movementSubscriber)
        {
            _movementPublisher = movementPublisher;
            this.movementSubscriber = movementSubscriber;
        }

        public UniTask PublishMovementAsync(MovementVO movementVO, CancellationToken token)
        {
            return _movementPublisher.PublishAsync(movementVO, token);
        }
    }
}