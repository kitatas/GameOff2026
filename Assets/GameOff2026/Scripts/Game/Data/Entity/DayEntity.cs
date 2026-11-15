using GameOff2026.Common.Data.Entity;

namespace GameOff2026.Game.Data.Entity
{
    public sealed class DayEntity : BaseEntity<int>
    {
        public void Reset() => Set(1);

        public void Increment() => Set(value + 1);
    }
}