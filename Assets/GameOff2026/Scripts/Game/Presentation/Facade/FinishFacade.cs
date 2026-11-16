using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Presentation.View;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class FinishFacade
    {
        private readonly ClearView _clearView;
        private readonly FailView _failView;

        public FinishFacade(ClearView clearView, FailView failView)
        {
            _clearView = clearView;
            _failView = failView;
        }

        public UniTask FadeAsync(FinishTransitionVO finishTransition, CancellationToken token)
        {
            var tween = finishTransition.transition.fade switch
            {
                Fade.In => FadeIn(finishTransition.type, finishTransition.transition.duration),
                Fade.Out => FadeOut(finishTransition.type, finishTransition.transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }

        private Tween FadeIn(FinishType type, float duration)
        {
            return type switch
            {
                FinishType.Clear => _clearView.FadeIn(duration),
                FinishType.Fail => _failView.FadeIn(duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FINISH),
            };
        }

        private Tween FadeOut(FinishType type, float duration)
        {
            return type switch
            {
                FinishType.Clear => _clearView.FadeOut(duration),
                FinishType.Fail => _failView.FadeOut(duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FINISH),
            };
        }
    }
}