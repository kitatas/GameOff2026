using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Utility;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameClearState : BaseGameState
    {
        private readonly FinishUseCase _finishUseCase;

        public GameClearState(FinishUseCase finishUseCase)
        {
            _finishUseCase = finishUseCase;
        }

        public override GameState state => GameState.Clear;

        public override async UniTask InitAsync(CancellationToken token)
        {
            await (
                _finishUseCase.InitAsync(FinishType.Clear, token)
            );
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _finishUseCase.FadeAsync(FinishType.Clear, Fade.In, token);
            await UniTaskHelper.DelayAsync(1.0f, token);
            await _finishUseCase.FadeAsync(FinishType.Clear, Fade.Out, token);

            return GameState.Send;
        }

        public override async UniTask ForceExitAsync(CancellationToken token)
        {
            await (
                _finishUseCase.FadeAsync(FinishType.Clear, Fade.Out, token)
            );
        }
    }
}