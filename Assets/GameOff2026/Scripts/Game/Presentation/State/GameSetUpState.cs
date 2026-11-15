using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameSetUpState : BaseGameState
    {
        private readonly DayUseCase _dayUseCase;
        private readonly FieldUseCase _fieldUseCase;
        private readonly TreasureUseCase _treasureUseCase;

        public GameSetUpState(DayUseCase dayUseCase, FieldUseCase fieldUseCase, TreasureUseCase treasureUseCase)
        {
            _dayUseCase = dayUseCase;
            _fieldUseCase = fieldUseCase;
            _treasureUseCase = treasureUseCase;
        }

        public override GameState state => GameState.SetUp;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            _dayUseCase.SetUp();
            _treasureUseCase.SetUp();
            await _fieldUseCase.BuildFieldAsync(token);
            return GameState.Input;
        }
    }
}