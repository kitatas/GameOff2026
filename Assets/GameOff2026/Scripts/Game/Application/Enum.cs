namespace GameOff2026.Game.Application
{
    public enum GameState
    {
        None = 0,
        Init = 1,
        SetUp = 2,
        Input = 3,
        Event = 4,
        Wander = 5,
        Fail = 6,
        Return = 7,
    }

    public enum InputType
    {
        None = 0,
        Move = 1,
        Shift = 2,
    }

    public enum Direction
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
    }

    public enum DateType
    {
        None = 0,
        Day = 1,
        Night = 2,
    }

    public enum FieldType
    {
        None = 0,
        Empty = 1,
        Treasure = 4,
        Block = 7,
        EnemyDay = 8,
        EnemyNight = 9,
        EnemyRouteActive = 10,
        EnemyRouteDeactive = 11,
    }

    public enum Rotation
    {
        None = 0,
        Angle0 = 1,
        Angle90 = 2,
        Angle180 = 3,
        Angle270 = 4,
    }

    public enum RenderType
    {
        None = 0,
        Refresh = 1,
    }

    public enum FinishType
    {
        None = 0,
        Clear = 1,
        Fail = 2,
    }
}