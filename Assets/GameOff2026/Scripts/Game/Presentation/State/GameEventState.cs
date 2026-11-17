using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameEventState : BaseGameState
    {
        private readonly EventUseCase _eventUseCase;
        private readonly FieldUseCase _fieldUseCase;
        private readonly StaminaUseCase _staminaUseCase;

        public GameEventState(EventUseCase eventUseCase, FieldUseCase fieldUseCase, StaminaUseCase staminaUseCase)
        {
            _eventUseCase = eventUseCase;
            _fieldUseCase = fieldUseCase;
            _staminaUseCase = staminaUseCase;
        }

        public override GameState state => GameState.Event;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            var result = await _eventUseCase.ApplyAsync(token);
            await _fieldUseCase.PublishRenderAsync(token);

            return _staminaUseCase.IsEmpty()
                ? GameState.Next
                : GameState.Wander;
        }
    }
}