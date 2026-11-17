using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using MessagePipe;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class DateUseCase
    {
        private readonly DateEntity _dateEntity;
        private readonly DatePorts _datePorts;

        public DateUseCase(DateEntity dateEntity, DatePorts datePorts)
        {
            _dateEntity = dateEntity;
            _datePorts = datePorts;
        }

        public IAsyncSubscriber<DateTransitionVO> dateTransition => _datePorts.dateTransitionSubscriber;

        public UniTask InitAsync(CancellationToken token)
        {
            _dateEntity.Set(DateType.Night);
            var transition = DateTransitionVO.Create(_dateEntity.value, Fade.InOut, 0.0f);
            return _datePorts.PublishDateTransitionAsync(transition, token);
        }

        public UniTask SetUpAsync(CancellationToken token)
        {
            _dateEntity.Reset();
            return PublishAsync(token);
        }

        public UniTask ShiftAsync(CancellationToken token)
        {
            _dateEntity.Shift();
            return PublishAsync(token);
        }

        private UniTask PublishAsync(CancellationToken token)
        {
            var transition = DateTransitionVO.Create(_dateEntity.value, Fade.InOut, DateConfig.FADE_DURATION);
            return _datePorts.PublishDateTransitionAsync(transition, token);
        }
    }
}