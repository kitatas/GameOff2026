using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Data.Entity;
using GameOff2026.Common.Domain.Ports;
using MessagePipe;

namespace GameOff2026.Common.Domain.UseCase
{
    public abstract class BasePubSubUseCase<T> : IDisposable
    {
        protected readonly BaseEntity<T> _entity;
        protected readonly BasePubSubPorts<T> _ports;

        protected BasePubSubUseCase(BaseEntity<T> entity, BasePubSubPorts<T> ports)
        {
            _entity = entity;
            _ports = ports;
        }

        public virtual IAsyncSubscriber<T> subscriber => _ports.subscriber;

        public virtual UniTask PublishAsync(T value, CancellationToken token)
        {
            _entity.Set(value);
            return _ports.PublishAsync(_entity.value, token);
        }

        public virtual void Dispose()
        {
        }
    }
}