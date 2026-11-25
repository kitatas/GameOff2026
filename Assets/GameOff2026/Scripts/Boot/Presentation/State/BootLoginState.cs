using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Boot.Application;
using GameOff2026.Boot.Domain.UseCase;
using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.UseCase;

namespace GameOff2026.Boot.Presentation.State
{
    public sealed class BootLoginState : BaseBootState
    {
        private readonly LoadingUseCase _loadingUseCase;
        private readonly LoginUseCase _loginUseCase;

        public BootLoginState(LoadingUseCase loadingUseCase, LoginUseCase loginUseCase)
        {
            _loadingUseCase = loadingUseCase;
            _loginUseCase = loginUseCase;
        }

        public override BootState state => BootState.Login;

        public override async UniTask<BootState> TickAsync(CancellationToken token)
        {
            await _loadingUseCase.FadeAsync(Fade.In, token);
            await _loginUseCase.InitAsync(token);
            await _loadingUseCase.FadeAsync(Fade.Out, token);

            return BootState.Load;
        }
    }
}