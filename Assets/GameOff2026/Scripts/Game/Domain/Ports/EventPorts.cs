using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using MessagePipe;

namespace GameOff2026.Game.Domain.Ports
{
    public sealed class EventPorts
    {
        public readonly IAsyncSubscriber<EventResultVO> eventResultSubscriber;
        private readonly IAsyncPublisher<EventResultVO> _eventResultPublisher;

        public EventPorts(IAsyncSubscriber<EventResultVO> eventResultSubscriber,
            IAsyncPublisher<EventResultVO> eventResultPublisher)
        {
            this.eventResultSubscriber = eventResultSubscriber;
            _eventResultPublisher = eventResultPublisher;
        }

        public UniTask PublishEventResultAsync(EventResultVO eventResult, CancellationToken token)
        {
            return _eventResultPublisher.PublishAsync(eventResult, token);
        }
    }
}