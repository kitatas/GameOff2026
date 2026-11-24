using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Boot.Application;

namespace GameOff2026.Boot.Presentation.State
{
    public sealed class BootInitState : BaseBootState
    {
        public override BootState state => BootState.Init;

        public override UniTask<BootState> TickAsync(CancellationToken token)
        {
            return UniTask.FromResult(BootState.Splash);
        }
    }
}