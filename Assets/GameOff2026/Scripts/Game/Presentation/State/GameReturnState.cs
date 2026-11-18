using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameReturnState : BaseGameState
    {
        public override GameState state => GameState.Return;

        public override UniTask<GameState> TickAsync(CancellationToken token)
        {
            // TODO
            return UniTask.FromResult(GameState.SetUp);
        }
    }
}