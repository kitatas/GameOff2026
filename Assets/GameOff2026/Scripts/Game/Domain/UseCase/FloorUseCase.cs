using System;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Repository;
using R3;
using UnityEngine;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class FloorUseCase : IDisposable
    {
        private readonly FloorEntity _floorEntity;
        private readonly TreasurePriceRateEntity _treasurePriceRateEntity;
        private readonly TreasurePriceRepository _treasurePriceRepository;
        private readonly ReactiveProperty<int> _floor;

        public FloorUseCase(FloorEntity floorEntity, TreasurePriceRateEntity treasurePriceRateEntity,
            TreasurePriceRepository treasurePriceRepository)
        {
            _floorEntity = floorEntity;
            _treasurePriceRateEntity = treasurePriceRateEntity;
            _treasurePriceRepository = treasurePriceRepository;
            _floor = new ReactiveProperty<int>();
        }

        public ReadOnlyReactiveProperty<int> floor => _floor.ToReadOnlyReactiveProperty();

        public void Init()
        {
            _floorEntity.Reset();
            _floor.Value = _floorEntity.value;

            _treasurePriceRateEntity.Reset();
        }

        public void Increment()
        {
            _floorEntity.Increment();
            _floor.Value = _floorEntity.value;
        }

        public int GetTreasurePrice()
        {
            var treasurePrice = _treasurePriceRepository.Find(_floorEntity.value);
            return Mathf.CeilToInt(treasurePrice.price * _treasurePriceRateEntity.rateValue);
        }

        void IDisposable.Dispose()
        {
            _floor?.Dispose();
        }
    }
}