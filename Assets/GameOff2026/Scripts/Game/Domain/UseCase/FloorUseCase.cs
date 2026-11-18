using System;
using GameOff2026.Game.Data.Entity;
using R3;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class FloorUseCase : IDisposable
    {
        private readonly FloorEntity _floorEntity;
        private readonly ReactiveProperty<int> _floor;

        public FloorUseCase(FloorEntity floorEntity)
        {
            _floorEntity = floorEntity;
            _floor = new ReactiveProperty<int>();
        }

        public ReadOnlyReactiveProperty<int> floor => _floor.ToReadOnlyReactiveProperty();

        public void Init()
        {
            _floorEntity.Reset();
            _floor.Value = _floorEntity.value;
        }

        public void Increment()
        {
            _floorEntity.Increment();
            _floor.Value = _floorEntity.value;
        }

        void IDisposable.Dispose()
        {
            _floor?.Dispose();
        }
    }
}