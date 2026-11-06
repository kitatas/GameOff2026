using GameOff2026.Game.Application;

namespace GameOff2026.Game.Data.Entity
{
    public sealed class PlayerEntity
    {
        public SquareVO square { get; private set; } = new(FieldConfig.MIN_FILE, FieldConfig.MIN_RANK);

        public void SetSquare(SquareVO vo)
        {
            square = vo;
        }
    }
}