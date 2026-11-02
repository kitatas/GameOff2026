using GameOff2026.Common.Domain.Ports;
using GameOff2026.Game.Application;
using MessagePipe;

namespace GameOff2026.Game.Domain.Ports
{
    public sealed class GameStatePorts : BaseStatePorts<GameState>
    {
        public GameStatePorts(IAsyncSubscriber<GameState> subscriber, IAsyncPublisher<GameState> publisher) : base(
            subscriber, publisher)
        {
        }
    }
}