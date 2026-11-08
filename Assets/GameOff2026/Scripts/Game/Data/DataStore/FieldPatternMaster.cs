using GameOff2026.Game.Application;
using MasterMemory;
using MessagePack;

namespace GameOff2026.Game.Data.DataStore
{
    [MemoryTable(nameof(FieldPatternMaster)), MessagePackObject(true)]
    public sealed class FieldPatternMaster
    {
        [PrimaryKey(keyOrder: 0)] public int Id { get; }
        public int[] Types { get; }

        public FieldPatternMaster(int id, int[] types)
        {
            Id = id;
            Types = types;
        }

        public FieldPatternVO ToVO() => new(Id, Types);
    }
}