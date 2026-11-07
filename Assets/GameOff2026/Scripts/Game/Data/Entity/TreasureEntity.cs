using GameOff2026.Common.Data.Entity;

namespace GameOff2026.Game.Data.Entity
{
    public sealed class TreasureEntity : BaseEntity<int>
    {
        public void Add(int x) => Set(value + x);
        public void Reset() => Set(0);
    }
}