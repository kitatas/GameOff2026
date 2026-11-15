using GameOff2026.Game.Presentation.View;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class DayFacade
    {
        private readonly DayView _dayView;

        public DayFacade(DayView dayView)
        {
            _dayView = dayView;
        }

        public void Render(int value)
        {
            _dayView.Render(value);
        }
    }
}