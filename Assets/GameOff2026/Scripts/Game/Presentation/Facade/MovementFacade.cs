using GameOff2026.Game.Application;
using GameOff2026.Game.Presentation.View;
using R3;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class MovementFacade
    {
        private readonly MovementView _movementView;

        public MovementFacade(MovementView movementView)
        {
            _movementView = movementView;
        }

        public Observable<Direction> OnClickAnyDirectionAsObservable()
        {
            return _movementView.OnClickAnyDirectionAsObservable();
        }
    }
}