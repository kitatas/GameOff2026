using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace GameOff2026.Common.Domain.Ports
{
    public abstract class BasePubSubPorts<T>
    {
        public readonly IAsyncSubscriber<T> subscriber;
        private readonly IAsyncPublisher<T> _publisher;

        protected BasePubSubPorts(IAsyncSubscriber<T> subscriber, IAsyncPublisher<T> publisher)
        {
            this.subscriber = subscriber;
            _publisher = publisher;
        }

        public virtual UniTask PublishAsync(T message, CancellationToken token)
        {
            return _publisher.PublishAsync(message, token);
        }
    }
}