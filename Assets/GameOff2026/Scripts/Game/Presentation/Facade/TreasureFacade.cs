using GameOff2026.Game.Application;
using GameOff2026.Game.Presentation.View;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class TreasureFacade
    {
        private readonly TreasureView _treasureView;

        public TreasureFacade(TreasureView treasureView)
        {
            _treasureView = treasureView;
        }

        public void Render((int prev, int current) pair)
        {
            _treasureView.Render(pair.prev, pair.current, TreasureConfig.TWEEN_DURATION);
        }
    }
}