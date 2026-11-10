using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using MessagePipe;
using R3;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class DateUseCase : IDisposable
    {
        private readonly DateEntity _dateEntity;
        private readonly DatePorts _datePorts;
        private readonly Subject<Unit> _shift;

        public DateUseCase(DateEntity dateEntity, DatePorts datePorts)
        {
            _dateEntity = dateEntity;
            _datePorts = datePorts;
            _shift = new Subject<Unit>();
        }

        public IAsyncSubscriber<DateTransitionVO> dateTransition => _datePorts.dateTransitionSubscriber;

        public UniTask SetUpAsync(CancellationToken token)
        {
            _dateEntity.Reset();
            return PublishAsync(token);
        }

        public async UniTask ShiftAsync(CancellationToken token)
        {
            await _shift.FirstAsync(token);
            _dateEntity.Shift();
            await PublishAsync(token);
        }

        private UniTask PublishAsync(CancellationToken token)
        {
            var transition = DateTransitionVO.Create(_dateEntity.value, Fade.InOut, DateConfig.FADE_DURATION);
            return _datePorts.PublishDateTransitionAsync(transition, token);
        }

        public void HandleShift()
        {
            // TODO: check input state
            _shift?.OnNext(Unit.Default);
        }

        void IDisposable.Dispose()
        {
            _shift?.Dispose();
        }
    }
}