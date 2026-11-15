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
        private readonly DayUseCase _dayUseCase;
        private readonly FieldUseCase _fieldUseCase;
        private readonly InputUseCase _inputUseCase;
        private readonly MovementUseCase _movementUseCase;

        public GameInputState(DateUseCase dateUseCase, DayUseCase dayUseCase, FieldUseCase fieldUseCase,
            InputUseCase inputUseCase, MovementUseCase movementUseCase)
        {
            _dateUseCase = dateUseCase;
            _dayUseCase = dayUseCase;
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
            return await (input.input switch
            {
                InputType.Move => ExecMoveAsync(input.direction, token),
                InputType.Shift => ExecShiftAsync(token),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_INPUT),
            });
        }

        private async UniTask<GameState> ExecMoveAsync(Direction direction, CancellationToken token)
        {
            await (
                _movementUseCase.MoveAsync(direction, token)
            );
            return GameState.Event;
        }

        private async UniTask<GameState> ExecShiftAsync(CancellationToken token)
        {
            await _dateUseCase.ShiftAsync(token);
            await (
                _fieldUseCase.ShiftAsync(token)
            );

            // TODO: Finish
            if (_dayUseCase.IsMaxDay()) return GameState.None;

            _dayUseCase.Dawn();
            return GameState.Input;
        }
    }
}