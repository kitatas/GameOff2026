using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.Ports;
using MessagePipe;

namespace GameOff2026.Common.Domain.UseCase
{
    public sealed class LoadingUseCase
    {
        private readonly LoadingPorts _loadingPorts;

        public LoadingUseCase(LoadingPorts loadingPorts)
        {
            _loadingPorts = loadingPorts;
        }

        public IAsyncSubscriber<LoadingTransitionVO> transition => _loadingPorts.loadingTransitionSubscriber;

        public UniTask InitAsync(CancellationToken token)
        {
            var loadingTransition = LoadingTransitionVO.Create(Fade.Out, 0.0f);
            return _loadingPorts.PublishLoadingTransitionAsync(loadingTransition, token);
        }

        public UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            var loadingTransition = LoadingTransitionVO.Create(fade, LoadingConfig.FADE_DURATION);
            return _loadingPorts.PublishLoadingTransitionAsync(loadingTransition, token);
        }
    }
}