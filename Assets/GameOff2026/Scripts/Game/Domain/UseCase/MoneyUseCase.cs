using System;
using GameOff2026.Game.Data.Entity;
using R3;
using UnityEngine;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class MoneyUseCase : IDisposable
    {
        private readonly MoneyEntity _moneyEntity;
        private readonly ShopPriceRateEntity _shopPriceRateEntity;
        private readonly ReactiveProperty<int> _money;

        public MoneyUseCase(MoneyEntity moneyEntity, ShopPriceRateEntity shopPriceRateEntity)
        {
            _moneyEntity = moneyEntity;
            _shopPriceRateEntity = shopPriceRateEntity;
            _money = new ReactiveProperty<int>();
        }

        public ReadOnlyReactiveProperty<int> money => _money.ToReadOnlyReactiveProperty();

        public void Init()
        {
            _moneyEntity.Reset();
            _money.Value = _moneyEntity.value;

            _shopPriceRateEntity.Reset();
        }

        public void Add(int value)
        {
            _moneyEntity.Add(value);
            _money.Value = _moneyEntity.value;
        }

        public void Pay(int value)
        {
            var payment = Mathf.CeilToInt(value * _shopPriceRateEntity.rateValue);
            _moneyEntity.Subtract(payment);
            _money.Value = _moneyEntity.value;
        }

        void IDisposable.Dispose()
        {
            _money?.Dispose();
        }
    }
}