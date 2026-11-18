using GameOff2026.Common.Application;
using GameOff2026.Game.Presentation.View;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class FloorFacade
    {
        private readonly FloorView _floorView;

        public FloorFacade(FloorView floorView)
        {
            _floorView = floorView;
        }

        public void Render((int prev, int current) pair)
        {
            _floorView.Render(pair.prev, pair.current, UiConfig.TWEEN_DURATION);
        }
    }
}