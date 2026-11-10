using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using MessagePipe;

namespace GameOff2026.Game.Domain.Ports
{
    public sealed class DatePorts
    {
        public readonly IAsyncSubscriber<DateTransitionVO> dateTransitionSubscriber;
        private readonly IAsyncPublisher<DateTransitionVO> _dateTransitionPublisher;

        public DatePorts(IAsyncSubscriber<DateTransitionVO> dateTransitionSubscriber,
            IAsyncPublisher<DateTransitionVO> dateTransitionPublisher)
        {
            this.dateTransitionSubscriber = dateTransitionSubscriber;
            _dateTransitionPublisher = dateTransitionPublisher;
        }

        public UniTask PublishDateTransitionAsync(DateTransitionVO dateTransition, CancellationToken token)
        {
            return _dateTransitionPublisher.PublishAsync(dateTransition, token);
        }
    }
}