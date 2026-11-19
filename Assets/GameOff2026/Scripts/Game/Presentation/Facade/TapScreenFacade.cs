using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Presentation.View;
using R3;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class TapScreenFacade
    {
        private readonly TapScreenView _tapScreenView;

        public TapScreenFacade(TapScreenView tapScreenView)
        {
            _tapScreenView = tapScreenView;
        }

        public Observable<Unit> OnTapAsObservable() => _tapScreenView.tap;

        public UniTask FadeAsync(TapScreenTransitionVO tapScreenTransition, CancellationToken token)
        {
            var tween = tapScreenTransition.transition.fade switch
            {
                Fade.In => _tapScreenView.FadeIn(tapScreenTransition.transition.duration),
                Fade.Out => _tapScreenView.FadeOut(tapScreenTransition.transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}