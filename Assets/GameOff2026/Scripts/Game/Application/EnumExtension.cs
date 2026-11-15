using FastEnumUtility;
using GameOff2026.Common.Application;
using UnityEngine;

namespace GameOff2026.Game.Application
{
    public static class EnumExtension
    {
        public static DateType ToDateType(this int self)
        {
            return FastEnum.IsDefined<DateType>(self)
                ? (DateType)self
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_DATE);
        }

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
            const int max = FieldConfig.HALF_FILE - 1;
            return self switch
            {
                Rotation.Angle0 => (file, rank),
                Rotation.Angle90 => (max - rank, file),
                Rotation.Angle180 => (max - file, max - rank),
                Rotation.Angle270 => (rank, max - file),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_ROTATION),
            };
        }

        public static DateType Shift(this DateType self)
        {
            return self switch
            {
                DateType.Day => DateType.Night,
                DateType.Night => DateType.Day,
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_DATE),
            };
        }

        public static bool IsEnemy(this FieldType self)
        {
            return self is FieldType.EnemyDay or FieldType.EnemyNight;
        }

        public static SquareVO[] Arounds(this SquareVO self)
        {
            return new[]
            {
                new SquareVO(self.file - 1, self.rank),
                new SquareVO(self.file + 1, self.rank),
                new SquareVO(self.file, self.rank - 1),
                new SquareVO(self.file, self.rank + 1),
            };
        }

        public static FieldType ToEnemy(this DateType self)
        {
            return self switch
            {
                DateType.Day => FieldType.EnemyDay,
                DateType.Night => FieldType.EnemyNight,
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_DATE),
            };
        }

        public static bool IsRoute(this FieldType self)
        {
            return self is FieldType.EnemyRouteActive or FieldType.EnemyRouteDeactive;
        }

        public static FieldType ShiftRoute(this FieldType self)
        {
            return self switch
            {
                FieldType.EnemyRouteActive => FieldType.EnemyRouteDeactive,
                FieldType.EnemyRouteDeactive => FieldType.EnemyRouteActive,
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD),
            };
        }

        public static DateType ToDate(this FieldType self)
        {
            return self switch
            {
                FieldType.EnemyDay => DateType.Day,
                FieldType.EnemyNight => DateType.Night,
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD),
            };
        }
    }
}