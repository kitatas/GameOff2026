using GameOff2026.Game.Application;
using GameOff2026.Game.Presentation.View;
using GameOff2026.Game.Presentation.View.Button;
using R3;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class InputFacade
    {
        private readonly DateShiftButtonView _dateShiftButtonView;
        private readonly MovementView _movementView;

        public InputFacade(DateShiftButtonView dateShiftButtonView, MovementView movementView)
        {
            _dateShiftButtonView = dateShiftButtonView;
            _movementView = movementView;
        }

        public Observable<InputVO> InputAnyAsObservable()
        {
            return _dateShiftButtonView.click
                .Select(_ => new InputVO(InputType.Shift, Direction.None))
                .Merge(_movementView
                    .OnClickAnyDirectionAsObservable()
                    .Select(x => new InputVO(InputType.Move, x)));
        }
    }
}