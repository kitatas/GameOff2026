using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameInputState : BaseGameState
    {
        private readonly DateUseCase _dateUseCase;
        private readonly MovementUseCase _movementUseCase;

        public GameInputState(DateUseCase dateUseCase, MovementUseCase movementUseCase)
        {
            _dateUseCase = dateUseCase;
            _movementUseCase = movementUseCase;
        }

        public override GameState state => GameState.Input;

        public override async UniTask InitAsync(CancellationToken token)
        {
            await (
                _dateUseCase.InitAsync(token)
            );
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _movementUseCase.PublishMovableDirectionsAsync(token);

            var index = await UniTask.WhenAny(
                _dateUseCase.ShiftAsync(token),
                _movementUseCase.InputAsync(token)
            );

            // TODO: date change event
            if (index == 0) return GameState.Event;

            await _movementUseCase.MoveAsync(token);

            return GameState.Event;
        }
    }
}