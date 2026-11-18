using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameInitState : BaseGameState
    {
        private readonly DateUseCase _dateUseCase;
        private readonly DayUseCase _dayUseCase;
        private readonly MoneyUseCase _moneyUseCase;

        public GameInitState(DateUseCase dateUseCase, DayUseCase dayUseCase, MoneyUseCase moneyUseCase)
        {
            _dateUseCase = dateUseCase;
            _dayUseCase = dayUseCase;
            _moneyUseCase = moneyUseCase;
        }

        public override GameState state => GameState.Init;

        public override async UniTask EnterAsync(CancellationToken token)
        {
            _moneyUseCase.Init();
            _dayUseCase.Init();
            await _dateUseCase.InitAsync(token);
        }

        public override UniTask<GameState> TickAsync(CancellationToken token)
        {
            // TODO
            return UniTask.FromResult(GameState.Return);
        }
    }
}