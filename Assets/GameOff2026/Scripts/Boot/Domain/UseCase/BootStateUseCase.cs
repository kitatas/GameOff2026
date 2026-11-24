using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Boot.Application;
using GameOff2026.Boot.Data.Entity;
using GameOff2026.Boot.Domain.Ports;
using GameOff2026.Common.Data.Entity;
using GameOff2026.Common.Domain.UseCase;

namespace GameOff2026.Boot.Domain.UseCase
{
    public sealed class BootStateUseCase : BaseStateUseCase<BootState>
    {
        public BootStateUseCase(BootStateEntity entity, RetryCountEntity retryCountEntity, BootStatePorts ports) : base(
            entity, retryCountEntity, ports)
        {
        }

        public override UniTask InitAsync(CancellationToken token)
        {
            return PublishAsync(BootState.Init, token);
        }

        public override UniTask PublishAsync(BootState value, CancellationToken token)
        {
            if (value is BootState.None) return UniTask.Yield(token);
            return base.PublishAsync(value, token);
        }
    }
}