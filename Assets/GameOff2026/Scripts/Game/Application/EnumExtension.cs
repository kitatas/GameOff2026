using GameOff2026.Common.Application;
using UnityEngine;

namespace GameOff2026.Game.Application
{
    public static class EnumExtension
    {
        public static Vector2Int ToVector2(this Direction self)
        {
            return self switch
            {
                Direction.Up => Vector2Int.up,
                Direction.Down => Vector2Int.down,
                Direction.Left => Vector2Int.left,
                Direction.Right => Vector2Int.right,
                _ => throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_DIRECTION),
            };
        }

        public static SquareVO ToSquareVO(this Direction self)
        {
            return self switch
            {
                Direction.Up => new SquareVO(0, 1),
                Direction.Down => new SquareVO(0, -1),
                Direction.Left => new SquareVO(-1, 0),
                Direction.Right => new SquareVO(1, 0),
                _ => throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_DIRECTION),
            };
        }
    }
}