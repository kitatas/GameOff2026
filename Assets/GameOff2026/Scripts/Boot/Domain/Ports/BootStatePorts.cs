using GameOff2026.Boot.Application;
using GameOff2026.Common.Domain.Ports;
using MessagePipe;

namespace GameOff2026.Boot.Domain.Ports
{
    public sealed class BootStatePorts : BaseStatePorts<BootState>
    {
        public BootStatePorts(IAsyncSubscriber<BootState> subscriber, IAsyncPublisher<BootState> publisher) : base(
            subscriber, publisher)
        {
        }
    }
}