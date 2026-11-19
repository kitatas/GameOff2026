using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using MessagePipe;

namespace GameOff2026.Game.Domain.Ports
{
    public sealed class TapScreenPorts
    {
        public readonly IAsyncSubscriber<TapScreenTransitionVO> tapScreenTransitionSubscriber;
        private readonly IAsyncPublisher<TapScreenTransitionVO> _tapScreenTransitionPublisher;

        public TapScreenPorts(IAsyncSubscriber<TapScreenTransitionVO> tapScreenTransitionSubscriber,
            IAsyncPublisher<TapScreenTransitionVO> tapScreenTransitionPublisher)
        {
            this.tapScreenTransitionSubscriber = tapScreenTransitionSubscriber;
            _tapScreenTransitionPublisher = tapScreenTransitionPublisher;
        }

        public UniTask PublishTapScreenAsync(TapScreenTransitionVO tapScreen, CancellationToken token)
        {
            return _tapScreenTransitionPublisher.PublishAsync(tapScreen, token);
        }
    }
}