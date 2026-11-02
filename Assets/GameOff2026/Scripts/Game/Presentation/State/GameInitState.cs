using GameOff2026.Game.Application;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameInitState : BaseGameState
    {
        public override GameState state => GameState.Init;
    }
}