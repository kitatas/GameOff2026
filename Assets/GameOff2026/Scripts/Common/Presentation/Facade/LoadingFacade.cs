using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Presentation.View;

namespace GameOff2026.Common.Presentation.Facade
{
    public sealed class LoadingFacade
    {
        private readonly LoadingView _loadingView;

        public LoadingFacade(LoadingView loadingView)
        {
            _loadingView = loadingView;
        }

        public UniTask FadeAsync(LoadingTransitionVO loadingTransition, CancellationToken token)
        {
            var tween = loadingTransition.transition.fade switch
            {
                Fade.In => _loadingView.FadeIn(loadingTransition.transition.duration),
                Fade.Out => _loadingView.FadeOut(loadingTransition.transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}