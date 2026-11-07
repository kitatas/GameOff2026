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

        public GameEventState(EventUseCase eventUseCase, FieldUseCase fieldUseCase)
        {
            _eventUseCase = eventUseCase;
            _fieldUseCase = fieldUseCase;
        }

        public override GameState state => GameState.Event;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            var result = await _eventUseCase.ApplyAsync(token);
            await _fieldUseCase.PublishRenderAsync(token);

            return result.isGoal
                ? GameState.SetUp
                : GameState.Input;
        }
    }
}