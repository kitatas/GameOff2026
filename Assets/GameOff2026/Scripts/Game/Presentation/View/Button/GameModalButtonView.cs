using GameOff2026.Common.Application;
using GameOff2026.Common.Presentation.View.Button;
using GameOff2026.Game.Application;

namespace GameOff2026.Game.Presentation.View.Button
{
    public sealed class GameModalButtonView : BaseModalButtonView<GameModalType>
    {
        protected override BaseModalVO<GameModalType> modal => new GameModalVO(modalType, fadeType);
        public GameModalType type => modal.type;
    }
}