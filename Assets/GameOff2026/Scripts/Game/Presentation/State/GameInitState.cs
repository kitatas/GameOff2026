using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameInitState : BaseGameState
    {
        private readonly DateUseCase _dateUseCase;
        private readonly DayUseCase _dayUseCase;
        private readonly FloorUseCase _floorUseCase;
        private readonly HudRootUseCase _hudRootUseCase;
        private readonly MoneyUseCase _moneyUseCase;

        public GameInitState(DateUseCase dateUseCase, DayUseCase dayUseCase, FloorUseCase floorUseCase,
            HudRootUseCase hudRootUseCase, MoneyUseCase moneyUseCase)
        {
            _dateUseCase = dateUseCase;
            _dayUseCase = dayUseCase;
            _floorUseCase = floorUseCase;
            _hudRootUseCase = hudRootUseCase;
            _moneyUseCase = moneyUseCase;
        }

        public override GameState state => GameState.Init;

        public override async UniTask InitAsync(CancellationToken token)
        {
            await (
                _hudRootUseCase.InitAsync(token)
            );
        }

        public override async UniTask EnterAsync(CancellationToken token)
        {
            _floorUseCase.Init();
            _moneyUseCase.Init();
            _dayUseCase.Init();

            await (
                _dateUseCase.InitAsync(token),
                _hudRootUseCase.FadeAsync(Fade.Out, token)
            );
        }

        public override UniTask<GameState> TickAsync(CancellationToken token)
        {
            // TODO
            return UniTask.FromResult(GameState.Return);
        }

        public override async UniTask ExitAsync(CancellationToken token)
        {
            await (
                _hudRootUseCase.FadeAsync(Fade.In, token)
            );
        }
    }
}