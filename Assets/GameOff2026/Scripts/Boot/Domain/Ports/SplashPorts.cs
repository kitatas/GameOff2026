using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Boot.Application;
using MessagePipe;

namespace GameOff2026.Boot.Domain.Ports
{
    public sealed class SplashPorts
    {
        public readonly IAsyncSubscriber<SplashTransitionVO> splashTransitionSubscriber;
        private readonly IAsyncPublisher<SplashTransitionVO> _splashTransitionPublisher;

        public SplashPorts(IAsyncSubscriber<SplashTransitionVO> splashTransitionSubscriber,
            IAsyncPublisher<SplashTransitionVO> splashTransitionPublisher)
        {
            this.splashTransitionSubscriber = splashTransitionSubscriber;
            _splashTransitionPublisher = splashTransitionPublisher;
        }

        public UniTask PublishSplashTransitionAsync(SplashTransitionVO splashTransition, CancellationToken token)
        {
            return _splashTransitionPublisher.PublishAsync(splashTransition, token);
        }
    }
}