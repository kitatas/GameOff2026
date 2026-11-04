using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Presentation.View;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class FieldFacade
    {
        private readonly FieldView _fieldView;

        public FieldFacade(FieldView fieldView)
        {
            _fieldView = fieldView;
        }

        public UniTask RenderAsync(RenderFieldSquareVO renderFieldSquare, CancellationToken token)
        {
            return _fieldView.Render(renderFieldSquare.squares)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}