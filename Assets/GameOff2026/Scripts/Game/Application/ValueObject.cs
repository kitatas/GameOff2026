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
}