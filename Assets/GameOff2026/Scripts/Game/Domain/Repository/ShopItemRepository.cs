using GameOff2026.Common.Application;
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

        public ShopItemVO Find(int id)
        {
            return _shopItemMasterTable.TryFindById(id, out var master)
                ? master.ToVO()
                : throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_SHOP_ITEM);
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