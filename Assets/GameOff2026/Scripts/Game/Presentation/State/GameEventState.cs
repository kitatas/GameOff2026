using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameEventState : BaseGameState
    {
        private readonly DayUseCase _dayUseCase;
        private readonly EventUseCase _eventUseCase;
        private readonly FieldUseCase _fieldUseCase;
        private readonly FloorUseCase _floorUseCase;
        private readonly StaminaUseCase _staminaUseCase;

        public GameEventState(DayUseCase dayUseCase, EventUseCase eventUseCase, FieldUseCase fieldUseCase,
            FloorUseCase floorUseCase, StaminaUseCase staminaUseCase)
        {
            _dayUseCase = dayUseCase;
            _eventUseCase = eventUseCase;
            _fieldUseCase = fieldUseCase;
            _floorUseCase = floorUseCase;
            _staminaUseCase = staminaUseCase;
        }

        public override GameState state => GameState.Event;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            var result = await _eventUseCase.ApplyAsync(token);
            await _fieldUseCase.PublishRenderAsync(token);

            var isEmptyTreasure = _fieldUseCase.IsEmptyTreasure();
            if (isEmptyTreasure) _floorUseCase.Increment();

            return
                _staminaUseCase.IsEmpty() ? 
                    _dayUseCase.IsMaxDay() ? GameState.Clear : GameState.Return :
                isEmptyTreasure ? GameState.SetUp :
                GameState.Input;
        }
    }
}