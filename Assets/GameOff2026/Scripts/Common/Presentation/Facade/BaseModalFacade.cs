using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Presentation.View.Button;
using GameOff2026.Common.Presentation.View.Modal;
using R3;

namespace GameOff2026.Common.Presentation.Facade
{
    public abstract class BaseModalFacade<T> where T : Enum
    {
        private readonly Dictionary<T, BaseModalView<T>> _modalMap;
        private readonly IEnumerable<BaseModalButtonView<T>> _buttons;

        public BaseModalFacade(IEnumerable<BaseModalView<T>> modals, IEnumerable<BaseModalButtonView<T>> buttons)
        {
            _modalMap = modals.ToDictionary(x => x.type, x => x);
            _buttons = buttons;
        }

        public IEnumerable<Observable<BaseModalVO<T>>> OnClickAsObservables()
            => _buttons.Select(x => x.clickModal);

        public UniTask FadeAsync(BaseModalTransitionVO<T> transition, CancellationToken token)
        {
            if (!_modalMap.TryGetValue(transition.type, out var modal))
                throw new QuitExceptionVO(ExceptionConfig.INVALID_MODAL);

            var tween = transition.transition.fade switch
            {
                Fade.In => modal.FadeIn(transition.transition.duration),
                Fade.Out => modal.FadeOut(transition.transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}