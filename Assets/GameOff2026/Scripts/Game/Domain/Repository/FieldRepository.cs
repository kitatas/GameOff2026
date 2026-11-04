using System.Collections.Generic;
using System.Linq;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.DataStore;

namespace GameOff2026.Game.Domain.Repository
{
    public sealed class FieldRepository
    {
        private readonly Dictionary<FieldType, FieldVO> _fieldMap;

        public FieldRepository(FieldTable fieldTable)
        {
            _fieldMap = fieldTable.all.ToDictionary(x => x.type, x => x.ToVO());
        }

        public FieldVO Find(FieldType type)
        {
            return _fieldMap.TryGetValue(type, out var vo)
                ? vo
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD);
        }
    }
}