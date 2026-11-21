using System.Collections.Generic;
using GameOff2026.Common.Presentation.Facade;
using GameOff2026.Game.Application;
using GameOff2026.Game.Presentation.View.Button;
using GameOff2026.Game.Presentation.View.Modal;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class GameModalFacade : BaseModalFacade<GameModalType>
    {
        public GameModalFacade(IEnumerable<BaseGameModalView> modals, IEnumerable<GameModalButtonView> buttons) : base(
            modals, buttons)
        {
        }
    }
}