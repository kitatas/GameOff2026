using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Boot.Application;
using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.UseCase;

namespace GameOff2026.Boot.Presentation.State
{
    public sealed class BootLoadState : BaseBootState
    {
        private readonly SceneUseCase _sceneUseCase;

        public BootLoadState(SceneUseCase sceneUseCase)
        {
            _sceneUseCase = sceneUseCase;
        }

        public override BootState state => BootState.Load;

        public override UniTask<BootState> TickAsync(CancellationToken token)
        {
            _sceneUseCase.LoadFade(SceneName.Game);
            return UniTask.FromResult(BootState.None);
        }
    }
}