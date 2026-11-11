using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Presentation.View;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class MovementFacade
    {
        private readonly MovementView _movementView;

        public MovementFacade(MovementView movementView)
        {
            _movementView = movementView;
        }

        public UniTask RenderButtonsAsync(MovableDirectionVO[] movableDirections, CancellationToken token)
        {
            _movementView.SetInteractableAll(movableDirections);
            return UniTask.Yield(token);
        }
    }
}