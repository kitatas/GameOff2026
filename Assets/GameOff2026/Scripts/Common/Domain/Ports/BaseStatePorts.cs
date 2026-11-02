using System;
using MessagePipe;

namespace GameOff2026.Common.Domain.Ports
{
    public abstract class BaseStatePorts<T> : BasePubSubPorts<T> where T : Enum
    {
        protected BaseStatePorts(IAsyncSubscriber<T> subscriber, IAsyncPublisher<T> publisher) : base(subscriber,
            publisher)
        {
        }
    }
}