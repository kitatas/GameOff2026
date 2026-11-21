using MessagePipe;

namespace GameOff2026.Common.Domain.Ports
{
    public abstract class BaseModalPorts<T> : BasePubSubPorts<T> where T : new()
    {
        protected BaseModalPorts(IAsyncSubscriber<T> subscriber, IAsyncPublisher<T> publisher) : base(subscriber,
            publisher)
        {
        }
    }
}