using GameOff2026.Game.Application;
using MasterMemory;
using MessagePack;

namespace GameOff2026.Game.Data.DataStore
{
    [MemoryTable(nameof(TreasurePriceMaster)), MessagePackObject(true)]
    public sealed class TreasurePriceMaster
    {
        [PrimaryKey(keyOrder: 0)] public int Floor { get; }
        public int Price { get; }

        public TreasurePriceMaster(int floor, int price)
        {
            Floor = floor;
            Price = price;
        }

        public TreasurePriceVO ToVO() => new(Floor, Price);
    }
}