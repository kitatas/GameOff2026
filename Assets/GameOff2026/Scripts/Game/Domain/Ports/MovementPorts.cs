using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using MessagePipe;

namespace GameOff2026.Game.Domain.Ports
{
    public sealed class MovementPorts
    {
        public readonly IAsyncSubscriber<MovementVO> movementSubscriber;
        public readonly IAsyncSubscriber<MovableDirectionVO[]> movableDirectionsSubscriber;
        private readonly IAsyncPublisher<MovementVO> _movementPublisher;
        private readonly IAsyncPublisher<MovableDirectionVO[]> _movableDirectionsPublisher;

        public MovementPorts(IAsyncSubscriber<MovementVO> movementSubscriber,
            IAsyncSubscriber<MovableDirectionVO[]> movableDirectionsSubscriber,
            IAsyncPublisher<MovementVO> movementPublisher,
            IAsyncPublisher<MovableDirectionVO[]> movableDirectionsPublisher)
        {
            this.movementSubscriber = movementSubscriber;
            this.movableDirectionsSubscriber = movableDirectionsSubscriber;
            _movementPublisher = movementPublisher;
            _movableDirectionsPublisher = movableDirectionsPublisher;
        }

        public UniTask PublishMovementAsync(MovementVO movementVO, CancellationToken token)
        {
            return _movementPublisher.PublishAsync(movementVO, token);
        }

        public UniTask PublishMovableDirectionsAsync(MovableDirectionVO[] movableDirections, CancellationToken token)
        {
            return _movableDirectionsPublisher.PublishAsync(movableDirections, token);
        }
    }
}