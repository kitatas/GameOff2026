using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameInputState : BaseGameState
    {
        private readonly MovementUseCase _movementUseCase;

        public GameInputState(MovementUseCase movementUseCase)
        {
            _movementUseCase = movementUseCase;
        }

        public override GameState state => GameState.Input;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _movementUseCase.InputAsync(token);

            await _movementUseCase.MoveAsync(token);

            // TODO
            return GameState.Input;
        }
    }
}