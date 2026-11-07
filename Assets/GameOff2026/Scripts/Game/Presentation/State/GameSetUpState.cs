using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameSetUpState : BaseGameState
    {
        private readonly FieldUseCase _fieldUseCase;
        private readonly TreasureUseCase _treasureUseCase;

        public GameSetUpState(FieldUseCase fieldUseCase, TreasureUseCase treasureUseCase)
        {
            _fieldUseCase = fieldUseCase;
            _treasureUseCase = treasureUseCase;
        }

        public override GameState state => GameState.SetUp;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            _treasureUseCase.SetUp();
            await _fieldUseCase.BuildFieldAsync(token);
            return GameState.Input;
        }
    }
}