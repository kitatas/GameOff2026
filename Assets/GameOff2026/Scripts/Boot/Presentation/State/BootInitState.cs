using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Boot.Application;
using GameOff2026.Common.Domain.UseCase;

namespace GameOff2026.Boot.Presentation.State
{
    public sealed class BootInitState : BaseBootState
    {
        private readonly ExceptionUseCase _exceptionUseCase;
        private readonly LoadingUseCase _loadingUseCase;

        public BootInitState(ExceptionUseCase exceptionUseCase, LoadingUseCase loadingUseCase)
        {
            _exceptionUseCase = exceptionUseCase;
            _loadingUseCase = loadingUseCase;
        }

        public override BootState state => BootState.Init;

        public override async UniTask EnterAsync(CancellationToken token)
        {
            await (
                _exceptionUseCase.InitAsync(token),
                _loadingUseCase.InitAsync(token)
            );
        }

        public override UniTask<BootState> TickAsync(CancellationToken token)
        {
            return UniTask.FromResult(BootState.Splash);
        }
    }
}