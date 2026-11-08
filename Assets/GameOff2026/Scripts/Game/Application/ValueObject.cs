using GameOff2026.Common.Application;
using UnityEngine;

namespace GameOff2026.Game.Application
{
    public sealed class SquareVO
    {
        public readonly int file;
        public readonly int rank;

        public SquareVO(int file, int rank)
        {
            this.file = file;
            this.rank = rank;
        }

        public float localX => file - FieldConfig.MIN_FILE - 2.5f;
        public float localZ => rank - FieldConfig.MIN_RANK - 2.5f;
        public Vector2 point => new Vector2(localX, localZ);

        public bool IsEqual(SquareVO square) => file == square.file && rank == square.rank;
    }

    public sealed class MovementVO
    {
        public readonly SquareVO square;
        public readonly float duration;

        public MovementVO(SquareVO square, float duration)
        {
            this.square = square;
            this.duration = duration;
        }
    }

    public sealed class FieldVO
    {
        public readonly FieldType type;
        public readonly Sprite sprite;
        public readonly float weight;

        public FieldVO(FieldType type, Sprite sprite, float weight)
        {
            if (type is FieldType.None) throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD);
            if (sprite == null) throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD_SPRITE);
            if (weight <= 0.0f) throw new QuitExceptionVO(ExceptionConfig.INVALID_WEIGHT);

            this.type = type;
            this.sprite = sprite;
            this.weight = weight;
        }
    }

    public sealed class FieldSquareVO
    {
        public readonly SquareVO square;
        public readonly FieldVO field;

        public FieldSquareVO(SquareVO square, FieldVO field)
        {
            this.square = square;
            this.field = field;
        }

        public static FieldSquareVO Create(int file, int rank, FieldVO field)
        {
            return new FieldSquareVO(new SquareVO(file, rank), field);
        }
    }

    public sealed class RenderFieldSquareVO
    {
        public readonly RenderType type;
        public readonly FieldSquareVO[] squares;

        public RenderFieldSquareVO(RenderType type, FieldSquareVO[] squares)
        {
            if (type is RenderType.None) throw new QuitExceptionVO(ExceptionConfig.INVALID_RENDER);

            this.type = type;
            this.squares = squares;
        }
    }

    public sealed class MovableDirectionVO
    {
        public readonly Direction direction;
        public readonly bool isMovable;

        public MovableDirectionVO(Direction direction, bool isMovable)
        {
            if (direction is Direction.None) throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_DIRECTION);

            this.direction = direction;
            this.isMovable = isMovable;
        }
    }

    public sealed class EventResultVO
    {
        public readonly FieldType type;
        public readonly int treasureNum;
        public readonly bool isGoal;

        public EventResultVO(FieldType type, int treasureNum, bool isGoal)
        {
            if (type is FieldType.None) throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD);

            this.type = type;
            this.treasureNum = treasureNum;
            this.isGoal = isGoal;
        }
    }
}