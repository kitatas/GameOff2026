using System.Linq;
using FastEnumUtility;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.DataStore;
using GameOff2026.Game.Data.DataStore.Tables;
using UniEx;
using ZLinq;

namespace GameOff2026.Game.Domain.Repository
{
    public sealed class FieldPatternRepository
    {
        private readonly FieldPatternMasterTable _fieldPatternMasterTable;

        public FieldPatternRepository(MemoryDatabase memoryDatabase)
        {
            _fieldPatternMasterTable = memoryDatabase.FieldPatternMasterTable;
        }

        public FieldPatternVO[] GetRandom4(DateType date)
        {
            var targets = _fieldPatternMasterTable.All
                .AsValueEnumerable()
                .Where(x => x.Date == DateType.Day.ToInt32())
                .ToArray();

            return Enumerable.Range(0, FieldConfig.LOT_PATTERN_COUNT)
                .AsValueEnumerable()
                .Select(_ => targets.GetRandom().ToVO())
                .ToArray();
        }

        public FieldPatternVO Get(int id, DateType date, Rotation rotation)
        {
            return _fieldPatternMasterTable.TryFindByIdAndDate((id, DateType.Day.ToInt32()), out var master)
                ? master.ToVO(rotation)
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD_PATTERN);
        }
    }
}