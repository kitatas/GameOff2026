using System;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using R3;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class DayUseCase : IDisposable
    {
        private readonly DateEntity _dateEntity;
        private readonly DayEntity _dayEntity;
        private readonly ReactiveProperty<int> _day;

        public DayUseCase(DateEntity dateEntity, DayEntity dayEntity)
        {
            _dateEntity = dateEntity;
            _dayEntity = dayEntity;
            _day = new BindableReactiveProperty<int>();
        }

        public ReadOnlyReactiveProperty<int> day => _day.ToReadOnlyReactiveProperty();

        public void SetUp()
        {
            _dayEntity.Reset();
            _day.Value = _dayEntity.value;
        }

        public void Dawn()
        {
            if (_dateEntity.value is DateType.Day)
            {
                _dayEntity.Increment();
                _day.Value = _dayEntity.value;
            }
        }

        public bool IsMaxDay()
        {
            return
                _dateEntity.value is DateType.Night &&
                _dayEntity.value == DayConfig.MAX_DAY;
        }

        void IDisposable.Dispose()
        {
            _day?.Dispose();
        }
    }
}