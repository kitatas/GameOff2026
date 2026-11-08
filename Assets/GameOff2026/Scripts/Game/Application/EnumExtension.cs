using FastEnumUtility;
using GameOff2026.Common.Application;
using UnityEngine;

namespace GameOff2026.Game.Application
{
    public static class EnumExtension
    {
        public static FieldType ToFieldType(this int self)
        {
            return FastEnum.IsDefined<FieldType>(self)
                ? (FieldType)self
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD);
        }

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

        public static (int file, int rank) Rotate(this Rotation self, int file, int rank)
        {
            return self switch
            {
                Rotation.Angle0 => (rank, file),
                Rotation.Angle90 => (FieldConfig.HALF_FILE - 1 - file, rank),
                Rotation.Angle180 => (FieldConfig.HALF_RANK - 1 - rank, FieldConfig.HALF_FILE - 1 - file),
                Rotation.Angle270 => (file, FieldConfig.HALF_RANK - 1 - rank),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_ROTATION),
            };
        }
    }
}