namespace GameOff2026.Game.Application
{
    public sealed class FieldConfig
    {
        public const int MIN_FILE = 1;
        public const int MAX_FILE = 6;
        public const int MIN_RANK = 1;
        public const int MAX_RANK = 6;
    }

    public sealed class PlayerConfig
    {
        public const float MOVE_DURATION = 0.25f;

        public static readonly Direction[] DIRECTIONS =
        {
            Direction.Up,
            Direction.Down,
            Direction.Left,
            Direction.Right,
        };
    }
}