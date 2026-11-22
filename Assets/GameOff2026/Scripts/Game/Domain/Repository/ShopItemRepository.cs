using GameOff2026.Game.Application;
using GameOff2026.Game.Data.DataStore;
using GameOff2026.Game.Data.DataStore.Tables;
using ZLinq;

namespace GameOff2026.Game.Domain.Repository
{
    public sealed class ShopItemRepository
    {
        private readonly ShopItemMasterTable _shopItemMasterTable;

        public ShopItemRepository(MemoryDatabase memoryDatabase)
        {
            _shopItemMasterTable = memoryDatabase.ShopItemMasterTable;
        }

        public ShopItemVO[] GetAll()
        {
            return _shopItemMasterTable.All
                .AsValueEnumerable()
                .Select(x => x.ToVO())
                .ToArray();
        }
    }
}