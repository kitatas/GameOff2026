using GameOff2026.Common.Data.Entity;

namespace GameOff2026.Game.Data.Entity
{
    public sealed class StaminaEntity : BaseEntity<int>
    {
        public void Decrement() => Set(value - 1);
        public bool IsEmpty() => value == 0;
    }
}