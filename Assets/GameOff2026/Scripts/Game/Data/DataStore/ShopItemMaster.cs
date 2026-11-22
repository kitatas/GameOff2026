using GameOff2026.Game.Application;
using MasterMemory;
using MessagePack;

namespace GameOff2026.Game.Data.DataStore
{
    [MemoryTable(nameof(ShopItemMaster)), MessagePackObject(true)]
    public sealed class ShopItemMaster
    {
        [PrimaryKey(keyOrder: 0)] public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public int Price { get; }
        public bool IsUniq { get; }

        public ShopItemMaster(int id, string name, string description, int price, bool isUniq)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            IsUniq = isUniq;
        }

        public ShopItemVO ToVO() => new(Id, Name, Description, Price, IsUniq);
    }
}