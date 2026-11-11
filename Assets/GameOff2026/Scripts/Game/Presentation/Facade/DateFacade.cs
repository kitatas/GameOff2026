using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Presentation.View;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class DateFacade
    {
        private readonly DateView _dateView;

        public DateFacade(DateView dateView)
        {
            _dateView = dateView;
        }

        public UniTask RenderAsync(DateTransitionVO dateTransition, CancellationToken token)
        {
            var duration = dateTransition.transition.duration / 2.0f;
            var tween = dateTransition.date switch
            {
                DateType.Day => DOTween.Sequence()
                    .Append(_dateView.RenderNight(duration))
                    .Append(_dateView.RenderDay(duration)),
                DateType.Night => DOTween.Sequence()
                    .Append(_dateView.RenderDay(duration))
                    .Append(_dateView.RenderNight(duration)),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_DATE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}