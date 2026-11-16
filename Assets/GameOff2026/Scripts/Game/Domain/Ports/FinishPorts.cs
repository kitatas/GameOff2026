using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using MessagePipe;

namespace GameOff2026.Game.Domain.Ports
{
    public sealed class FinishPorts
    {
        public readonly IAsyncSubscriber<FinishTransitionVO> finishTransitionSubscriber;
        private readonly IAsyncPublisher<FinishTransitionVO> _finishTransitionPublisher;

        public FinishPorts(IAsyncSubscriber<FinishTransitionVO> finishTransitionSubscriber,
            IAsyncPublisher<FinishTransitionVO> finishTransitionPublisher)
        {
            this.finishTransitionSubscriber = finishTransitionSubscriber;
            _finishTransitionPublisher = finishTransitionPublisher;
        }

        public UniTask PublishFinishAsync(FinishTransitionVO finishTransition, CancellationToken token)
        {
            return _finishTransitionPublisher.PublishAsync(finishTransition, token);
        }
    }
}