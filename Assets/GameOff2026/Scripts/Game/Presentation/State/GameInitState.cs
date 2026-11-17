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

        public GameInitState(DateUseCase dateUseCase, DayUseCase dayUseCase)
        {
            _dateUseCase = dateUseCase;
            _dayUseCase = dayUseCase;
        }

        public override GameState state => GameState.Init;

        public override async UniTask EnterAsync(CancellationToken token)
        {
            _dayUseCase.Init();
            await _dateUseCase.InitAsync(token);
        }

        public override UniTask<GameState> TickAsync(CancellationToken token)
        {
            // TODO
            return UniTask.FromResult(GameState.SetUp);
        }
    }
}