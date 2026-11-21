using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.Ports;
using GameOff2026.Game.Application;
using MessagePipe;

namespace GameOff2026.Game.Domain.Ports
{
    public sealed class GameModalPorts : BasePubSubPorts<BaseModalTransitionVO<GameModalType>>
    {
        public GameModalPorts(IAsyncSubscriber<BaseModalTransitionVO<GameModalType>> subscriber,
            IAsyncPublisher<BaseModalTransitionVO<GameModalType>> publisher) : base(subscriber, publisher)
        {
        }
    }
}