using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using MessagePipe;

namespace GameOff2026.Common.Domain.Ports
{
    public sealed class LoadingPorts
    {
        public readonly IAsyncSubscriber<LoadingTransitionVO> loadingTransitionSubscriber;
        private readonly IAsyncPublisher<LoadingTransitionVO> _loadingTransitionPublisher;

        public LoadingPorts(IAsyncSubscriber<LoadingTransitionVO> loadingTransitionSubscriber,
            IAsyncPublisher<LoadingTransitionVO> loadingTransitionPublisher)
        {
            this.loadingTransitionSubscriber = loadingTransitionSubscriber;
            _loadingTransitionPublisher = loadingTransitionPublisher;
        }

        public UniTask PublishLoadingTransitionAsync(LoadingTransitionVO loadingTransition, CancellationToken token)
        {
            return _loadingTransitionPublisher.PublishAsync(loadingTransition, token);
        }
    }
}