using GameOff2026.Common.Application;
using GameOff2026.Game.Presentation.View;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class StaminaFacade
    {
        private readonly StaminaView _staminaView;

        public StaminaFacade(StaminaView staminaView)
        {
            _staminaView = staminaView;
        }

        public void Render((int prev, int current) pair)
        {
            _staminaView.Render(pair.prev, pair.current, UiConfig.TWEEN_DURATION);
        }

        public void RenderMax((int prev, int current) pair)
        {
            _staminaView.RenderMax(pair.prev, pair.current, UiConfig.TWEEN_DURATION);
        }
    }
}