using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameOff2026.Game.Application;
using GameOff2026.Game.Presentation.View;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class FieldFacade
    {
        private readonly DoorView _doorView;
        private readonly FieldView _fieldView;

        public FieldFacade(DoorView doorView, FieldView fieldView)
        {
            _doorView = doorView;
            _fieldView = fieldView;
        }

        public UniTask RenderAsync(RenderFieldSquareVO renderFieldSquare, CancellationToken token)
        {
            return DOTween.Sequence()
                .Join(_doorView.Render(renderFieldSquare.squares))
                .Join(_fieldView.Render(renderFieldSquare.squares))
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}