using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameReturnState : BaseGameState
    {
        private readonly DateUseCase _dateUseCase;
        private readonly DayUseCase _dayUseCase;
        private readonly StaminaUseCase _staminaUseCase;
        private readonly TreasureUseCase _treasureUseCase;

        public GameReturnState(DateUseCase dateUseCase, DayUseCase dayUseCase, StaminaUseCase staminaUseCase,
            TreasureUseCase treasureUseCase)
        {
            _dateUseCase = dateUseCase;
            _dayUseCase = dayUseCase;
            _staminaUseCase = staminaUseCase;
            _treasureUseCase = treasureUseCase;
        }

        public override GameState state => GameState.Return;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _dateUseCase.ShiftAsync(token);

            _dayUseCase.Dawn();
            _staminaUseCase.Refresh();
            _treasureUseCase.SetUp();

            return GameState.SetUp;
        }
    }
}