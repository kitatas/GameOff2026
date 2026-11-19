using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using MessagePipe;

namespace GameOff2026.Game.Domain.Ports
{
    public sealed class HudRootPorts
    {
        public readonly IAsyncSubscriber<HudRootTransitionVO> hudRootTransitionSubscriber;
        private readonly IAsyncPublisher<HudRootTransitionVO> _hudRootTransitionPublisher;

        public HudRootPorts(IAsyncSubscriber<HudRootTransitionVO> hudRootTransitionSubscriber,
            IAsyncPublisher<HudRootTransitionVO> hudRootTransitionPublisher)
        {
            this.hudRootTransitionSubscriber = hudRootTransitionSubscriber;
            _hudRootTransitionPublisher = hudRootTransitionPublisher;
        }

        public UniTask PublishHudRootTransitionAsync(HudRootTransitionVO parameterTransition, CancellationToken token)
        {
            return _hudRootTransitionPublisher.PublishAsync(parameterTransition, token);
        }
    }
}