namespace GameOff2026.Game.Application
{
    public sealed class FieldConfig
    {
        public const int MIN_FILE = 1;
        public const int MAX_FILE = 6;
        public const int HALF_FILE = MAX_FILE / 2;
        public const int MIN_RANK = 1;
        public const int MAX_RANK = 6;
        public const int HALF_RANK = MAX_RANK / 2;
        public const int LOT_PATTERN_COUNT = 4;

        public static readonly Rotation[] ROTATIONS =
        {
            Rotation.Angle0,
            Rotation.Angle90,
            Rotation.Angle180,
            Rotation.Angle270,
        };
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

    public sealed class TreasureConfig
    {
        public const float TWEEN_DURATION = 0.25f;
    }

    public sealed class DateConfig
    {
        public const float FADE_DURATION = 0.25f;
    }

    public sealed class DayConfig
    {
        public const int MAX_DAY = 7;
    }
}