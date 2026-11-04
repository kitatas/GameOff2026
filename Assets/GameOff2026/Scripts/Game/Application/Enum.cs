namespace GameOff2026.Game.Application
{
    public enum GameState
    {
        None = 0,
        Init = 1,
        Input = 2,
    }

    public enum Direction
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
    }

    public enum FieldType
    {
        None = 0,
        Ground = 1,
    }

    public enum RenderType
    {
        None = 0,
        Refresh = 1,
    }
}