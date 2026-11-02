using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Data.Entity;
using GameOff2026.Common.Domain.Ports;
using R3;

namespace GameOff2026.Common.Domain.UseCase
{
    public abstract class BaseStateUseCase<T> : BasePubSubUseCase<T> where T : Enum
    {
        private readonly RetryCountEntity _retryCountEntity;
        private readonly BehaviorSubject<T> _forceChange;

        protected BaseStateUseCase(BaseEntity<T> entity, RetryCountEntity retryCountEntity, BaseStatePorts<T> ports) :
            base(entity, ports)
        {
            _retryCountEntity = retryCountEntity;
            _forceChange = new BehaviorSubject<T>(default);
        }

        public Observable<T> forceChange => _forceChange;
        public T forceChangeState => _forceChange.Value;

        public void ForceChange(T state)
        {
            if (_entity.IsEqual(state)) return;
            _forceChange?.OnNext(state);
        }

        public abstract UniTask InitAsync(CancellationToken token);

        public bool IsMaxRetry(T state)
        {
            _retryCountEntity.Update(_entity.IsEqual(state));
            return _retryCountEntity.IsMaxRetry();
        }

        public override void Dispose()
        {
            base.Dispose();
            _forceChange?.Dispose();
        }
    }
}