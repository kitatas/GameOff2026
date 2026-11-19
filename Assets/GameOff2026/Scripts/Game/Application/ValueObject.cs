using GameOff2026.Common.Application;
using UniEx;
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

        public EventResultVO(FieldType type, int treasureNum)
        {
            if (type is FieldType.None) throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD);

            this.type = type;
            this.treasureNum = treasureNum;
        }
    }

    public sealed class FieldPatternVO
    {
        public readonly int id;
        public readonly DateType date;
        public readonly FieldType[] types;
        public readonly Rotation rotation;

        public FieldPatternVO(int id, int date, int[] types)
            : this(id, date, types, FieldConfig.ROTATIONS.GetRandom())
        {
        }

        public FieldPatternVO(int id, int date, int[] types, Rotation rotation)
        {
            if (date == 0) throw new QuitExceptionVO(ExceptionConfig.INVALID_DATE);

            const int length = FieldConfig.HALF_FILE * FieldConfig.HALF_RANK;
            if (types.Length != length) throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD_LENGTH);

            if (rotation is Rotation.None) throw new QuitExceptionVO(ExceptionConfig.INVALID_ROTATION);

            this.id = id;
            this.date = date.ToDateType();
            this.types = new FieldType[length];
            this.rotation = rotation;

            for (int file = 0; file < FieldConfig.HALF_FILE; file++)
            {
                for (int rank = 0; rank < FieldConfig.HALF_RANK; rank++)
                {
                    var (nx, ny) = rotation.Rotate(file, rank);
                    var srcIndex = file * FieldConfig.HALF_RANK + rank;
                    var dstIndex = nx * FieldConfig.HALF_RANK + ny;
                    this.types[dstIndex] = types[srcIndex].ToFieldType();
                }
            }
        }
    }

    public sealed class DateTransitionVO
    {
        public readonly DateType date;
        public readonly TransitionVO transition;

        public DateTransitionVO(DateType date, TransitionVO transition)
        {
            if (date is DateType.None) throw new QuitExceptionVO(ExceptionConfig.INVALID_DATE);

            this.date = date;
            this.transition = transition;
        }

        public static DateTransitionVO Create(DateType date, Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new DateTransitionVO(date, transition);
        }
    }

    public sealed class InputVO
    {
        public readonly InputType input;
        public readonly Direction direction;

        public InputVO(InputType input, Direction direction)
        {
            if (input is InputType.None) throw new QuitExceptionVO(ExceptionConfig.INVALID_INPUT);
            if (input is InputType.Move && direction is Direction.None)
                throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_DIRECTION);

            this.input = input;
            this.direction = direction;
        }
    }

    public sealed class FinishTransitionVO
    {
        public readonly FinishType type;
        public readonly TransitionVO transition;

        public FinishTransitionVO(FinishType type, TransitionVO transition)
        {
            if (type is FinishType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_FINISH);

            this.type = type;
            this.transition = transition;
        }

        public static FinishTransitionVO Create(FinishType type, Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new FinishTransitionVO(type, transition);
        }
    }

    public sealed class TreasurePriceVO
    {
        public readonly int floor;
        public readonly int price;

        public TreasurePriceVO(int floor, int price)
        {
            this.floor = floor;
            this.price = price;
        }
    }

    public sealed class HudRootTransitionVO
    {
        public readonly TransitionVO transition;

        public HudRootTransitionVO(TransitionVO transition)
        {
            this.transition = transition;
        }

        public static HudRootTransitionVO Create(Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new HudRootTransitionVO(transition);
        }
    }

    public sealed class TapScreenTransitionVO
    {
        public readonly TransitionVO transition;

        public TapScreenTransitionVO(TransitionVO transition)
        {
            this.transition = transition;
        }

        public static TapScreenTransitionVO Create(Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new TapScreenTransitionVO(transition);
        }
    }
}