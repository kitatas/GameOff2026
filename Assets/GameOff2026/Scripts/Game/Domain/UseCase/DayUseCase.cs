using System;
using GameOff2026.Game.Data.Entity;
using R3;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class DayUseCase : IDisposable
    {
        private readonly DayEntity _dayEntity;
        private readonly ReactiveProperty<int> _day;

        public DayUseCase(DayEntity dayEntity)
        {
            _dayEntity = dayEntity;
            _day = new BindableReactiveProperty<int>();
        }

        public ReadOnlyReactiveProperty<int> day => _day.ToReadOnlyReactiveProperty();

        void IDisposable.Dispose()
        {
            _day?.Dispose();
        }
    }
}