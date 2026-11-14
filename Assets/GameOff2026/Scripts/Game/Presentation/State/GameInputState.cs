using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameInputState : BaseGameState
    {
        private readonly DateUseCase _dateUseCase;
        private readonly FieldUseCase _fieldUseCase;
        private readonly InputUseCase _inputUseCase;
        private readonly MovementUseCase _movementUseCase;

        public GameInputState(DateUseCase dateUseCase, FieldUseCase fieldUseCase, InputUseCase inputUseCase,
            MovementUseCase movementUseCase)
        {
            _dateUseCase = dateUseCase;
            _fieldUseCase = fieldUseCase;
            _inputUseCase = inputUseCase;
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

            var input = await _inputUseCase.InputAsync(token);
            await (input.input switch
            {
                InputType.Move => _movementUseCase.MoveAsync(input.direction, token),
                InputType.Shift => UniTask.WhenAll(_dateUseCase.ShiftAsync(token), _fieldUseCase.ShiftAsync(token)),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_INPUT),
            });

            // TODO: date change event

            return GameState.Event;
        }
    }
}