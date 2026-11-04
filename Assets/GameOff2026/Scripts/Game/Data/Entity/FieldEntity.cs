using System.Collections.Generic;
using GameOff2026.Game.Application;

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

        public void Add(FieldSquareVO fieldSquare)
        {
            _fieldSquares.Add(fieldSquare);
        }

        public FieldSquareVO[] fieldSquares => _fieldSquares.ToArray();
    }
}