using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Data.Entity;
using GameOff2026.Common.Domain.Ports;

namespace GameOff2026.Common.Domain.UseCase
{
    public abstract class BaseModalUseCase<T> : BasePubSubUseCase<BaseModalTransitionVO<T>> where T : Enum
    {
        public BaseModalUseCase(BaseEntity<BaseModalTransitionVO<T>> entity,
            BasePubSubPorts<BaseModalTransitionVO<T>> ports) : base(entity, ports)
        {
        }

        protected abstract BaseModalTransitionVO<T> GetModalTransition(BaseModalVO<T> modal);

        public UniTask FadeAsync(BaseModalVO<T> modal, CancellationToken token)
        {
            return PublishAsync(GetModalTransition(modal), token);
        }
    }
}