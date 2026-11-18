using GameOff2026.Common.Application;
using GameOff2026.Game.Presentation.View;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class MoneyFacade
    {
        private readonly MoneyView _moneyView;

        public MoneyFacade(MoneyView moneyView)
        {
            _moneyView = moneyView;
        }

        public void Render((int prev, int current) pair)
        {
            _moneyView.Render(pair.prev, pair.current, UiConfig.TWEEN_DURATION);
        }
    }
}