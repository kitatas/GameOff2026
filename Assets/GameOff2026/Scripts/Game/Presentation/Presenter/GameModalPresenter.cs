using GameOff2026.Common.Presentation.Presenter;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class GameModalPresenter : BaseModalPresenter<GameModalType>
    {
        public GameModalPresenter(GameModalUseCase modalUseCase, GameModalFacade modalFacade)
            : base(modalUseCase, modalFacade)
        {
        }
    }
}