using System;
using GameOff2026.Game.Data.Entity;
using R3;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class TreasureUseCase : IDisposable
    {
        private readonly TreasureEntity _treasureEntity;
        private readonly ReactiveProperty<int> _treasure;

        public TreasureUseCase(TreasureEntity treasureEntity)
        {
            _treasureEntity = treasureEntity;
            _treasure = new ReactiveProperty<int>(0);
        }

        public ReadOnlyReactiveProperty<int> treasure => _treasure.ToReadOnlyReactiveProperty();

        public void SetUp()
        {
            _treasureEntity.Reset();
            _treasure.Value = _treasureEntity.value;
        }

        public void Add(int value)
        {
            _treasureEntity.Add(value);
            _treasure.Value = _treasureEntity.value;
        }

        void IDisposable.Dispose()
        {
            _treasure?.Dispose();
        }
    }
}