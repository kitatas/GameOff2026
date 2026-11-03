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
    }
}