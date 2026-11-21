using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.UseCase;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class GameModalUseCase : BaseModalUseCase<GameModalType>
    {
        public GameModalUseCase(GameModalEntity entity, GameModalPorts ports) : base(entity, ports)
        {
        }

        protected override BaseModalTransitionVO<GameModalType> GetModalTransition(BaseModalVO<GameModalType> modal)
        {
            return GameModalTransitionVO.Create(modal as GameModalVO, GameModalConfig.FADE_DURATION);
        }
    }
}