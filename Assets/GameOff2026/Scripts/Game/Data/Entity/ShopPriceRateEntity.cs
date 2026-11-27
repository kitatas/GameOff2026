using GameOff2026.Common.Data.Entity;

namespace GameOff2026.Game.Data.Entity
{
    public sealed class ShopPriceRateEntity : BaseEntity<int>
    {
        public void Reset() => Set(100);
        public void Subtract(int x) => Set(value - x);
        public float rateValue => value / 100.0f;
    }
}