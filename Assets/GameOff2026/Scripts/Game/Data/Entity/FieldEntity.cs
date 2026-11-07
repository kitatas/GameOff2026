using System.Collections.Generic;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using UniEx;

namespace GameOff2026.Game.Data.Entity
{
    public sealed class FieldEntity
    {
        private readonly List<FieldSquareVO> _fieldSquares;

        public FieldEntity()
        {
            _fieldSquares = new List<FieldSquareVO>(FieldConfig.MAX_FILE * FieldConfig.MAX_RANK);
        }

        public void Clear()
        {
            _fieldSquares.Clear();
        }

        public void SetField(int index, FieldVO field)
        {
            if (_fieldSquares.TryGetValue(index, out var fieldSquare))
            {
                _fieldSquares[index] = new FieldSquareVO(fieldSquare.square, field);
            }
            else
            {
                throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD_INDEX);
            }
        }

        public void Add(FieldSquareVO fieldSquare)
        {
            _fieldSquares.Add(fieldSquare);
        }

        public FieldSquareVO[] fieldSquares => _fieldSquares.ToArray();

        public (FieldSquareVO fieldSquare, int index) Find(SquareVO square)
        {
            var index = _fieldSquares.FindIndex(x => x.square.IsEqual(square));
            return index == -1
                ? (null, index)
                : (_fieldSquares[index], index);
        }

        public bool IsMovable(SquareVO square)
        {
            var (fieldSquare, _) = Find(square);
            if (fieldSquare == null) return false;

            return fieldSquare.field.type switch
            {
                // TODO: 移動不可の field type
                FieldType.Wall => false,
                FieldType.WallDoor => false,
                _ => true,
            };
        }
    }
}