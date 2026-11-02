using System.Collections.Generic;
using GameOff2026.Common.Domain.UseCase;
using GameOff2026.Common.Presentation.Presenter;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.State;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class GameStatePresenter : BaseStatePresenter<GameState>
    {
        public GameStatePresenter(ExceptionUseCase exceptionUseCase, GameStateUseCase stateUseCase,
            IEnumerable<BaseGameState> states) : base(exceptionUseCase, stateUseCase, states)
        {
        }
    }
}