using System;
using GameOff2026.Game.Data.Entity;
using R3;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class MoneyUseCase : IDisposable
    {
        private readonly MoneyEntity _moneyEntity;
        private readonly ReactiveProperty<int> _money;

        public MoneyUseCase(MoneyEntity moneyEntity)
        {
            _moneyEntity = moneyEntity;
            _money = new ReactiveProperty<int>();
        }

        public ReadOnlyReactiveProperty<int> money => _money.ToReadOnlyReactiveProperty();

        public void Init()
        {
            _moneyEntity.Reset();
            _money.Value = _moneyEntity.value;
        }

        public void Add(int value)
        {
            _moneyEntity.Add(value);
            _money.Value = _moneyEntity.value;
        }

        void IDisposable.Dispose()
        {
            _money?.Dispose();
        }
    }
}