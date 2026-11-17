using System;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using R3;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class StaminaUseCase : IDisposable
    {
        private readonly StaminaEntity _staminaEntity;
        private readonly MaxStaminaEntity _maxStaminaEntity;
        private readonly ReactiveProperty<int> _stamina;
        private readonly ReactiveProperty<int> _maxStamina;

        public StaminaUseCase(StaminaEntity staminaEntity, MaxStaminaEntity maxStaminaEntity)
        {
            _staminaEntity = staminaEntity;
            _maxStaminaEntity = maxStaminaEntity;
            _stamina = new ReactiveProperty<int>();
            _maxStamina = new ReactiveProperty<int>();
        }

        public ReadOnlyReactiveProperty<int> stamina => _stamina.ToReadOnlyReactiveProperty();
        public ReadOnlyReactiveProperty<int> maxStamina => _maxStamina.ToReadOnlyReactiveProperty();

        public void SetUp()
        {
            _maxStaminaEntity.Set(PlayerConfig.MAX_STAMINA);
            _maxStamina.Value = _maxStaminaEntity.value;

            _staminaEntity.Set(_maxStaminaEntity.value);
            _stamina.Value = _staminaEntity.value;
        }

        public void Refresh()
        {
            _staminaEntity.Set(_maxStaminaEntity.value);
            _stamina.Value = _staminaEntity.value;
        }

        public void Decrease()
        {
            _staminaEntity.Decrement();
            _stamina.Value = _staminaEntity.value;
        }

        void IDisposable.Dispose()
        {
            _stamina?.Dispose();
            _maxStamina?.Dispose();
        }
    }
}