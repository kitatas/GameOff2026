using System.Collections.Generic;
using System.Linq;
using GameOff2026.Common.Application;
using GameOff2026.Common.Utility;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.DataStore;
using ZLinq;

namespace GameOff2026.Game.Domain.Repository
{
    public sealed class FieldRepository
    {
        private readonly Dictionary<FieldType, FieldVO[]> _fieldMap;

        public FieldRepository(FieldTable fieldTable)
        {
            _fieldMap = fieldTable.all
                .AsValueEnumerable()
                .GroupBy(x => x.type)
                .ToDictionary(x => x.Key, x => x.Select(y => y.ToVO()).ToArray());
        }

        public FieldVO Find(FieldType type)
        {
            return _fieldMap.TryGetValue(type, out var vos) && vos.Length > 0
                ? vos.WeightedRandom(x => x.weight)
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD);
        }
    }
}