using GameOff2026.Common.Data.Entity;

namespace GameOff2026.Game.Data.Entity
{
    public sealed class FloorEntity : BaseEntity<int>
    {
        public void Reset() => Set(0);
        public void Increment() => Set(value + 1);
    }
}