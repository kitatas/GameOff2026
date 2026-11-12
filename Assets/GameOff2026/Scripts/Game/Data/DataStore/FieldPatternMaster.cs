using GameOff2026.Game.Application;
using MasterMemory;
using MessagePack;

namespace GameOff2026.Game.Data.DataStore
{
    [MemoryTable(nameof(FieldPatternMaster)), MessagePackObject(true)]
    public sealed class FieldPatternMaster
    {
        [PrimaryKey(keyOrder: 0)] public int Id { get; }
        [PrimaryKey(keyOrder: 1)] public int Date { get; }
        public int[] Types { get; }

        public FieldPatternMaster(int id, int date, int[] types)
        {
            Id = id;
            Date = date;
            Types = types;
        }

        public FieldPatternVO ToVO() => new(Id, Date, Types);
        public FieldPatternVO ToVO(Rotation rotation) => new(Id, Date, Types, rotation);
    }
}