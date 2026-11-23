using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameSetUpState : BaseGameState
    {
        private readonly FieldUseCase _fieldUseCase;
        private readonly MovementUseCase _movementUseCase;

        public GameSetUpState(FieldUseCase fieldUseCase, MovementUseCase movementUseCase)
        {
            _fieldUseCase = fieldUseCase;
            _movementUseCase = movementUseCase;
        }

        public override GameState state => GameState.SetUp;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _movementUseCase.SetUpAsync(token);
            await _fieldUseCase.BuildFieldAsync(token);
            return GameState.Event;
        }
    }
}