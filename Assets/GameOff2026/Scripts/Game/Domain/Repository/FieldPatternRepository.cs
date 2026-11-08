using System.Linq;
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

        public FieldPatternVO[] GetRandom4()
        {
            return Enumerable.Range(0, 4)
                .AsValueEnumerable()
                .Select(_ => _fieldPatternMasterTable.All.GetRandom().ToVO())
                .ToArray();
        }
    }
}