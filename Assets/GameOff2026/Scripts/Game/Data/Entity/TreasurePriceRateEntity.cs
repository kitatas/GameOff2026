using GameOff2026.Common.Data.Entity;

namespace GameOff2026.Game.Data.Entity
{
    public sealed class TreasurePriceRateEntity : BaseEntity<int>
    {
        public void Add(int x) => Set(value + x);
        public float rateValue => value / 100.0f;
    }
}