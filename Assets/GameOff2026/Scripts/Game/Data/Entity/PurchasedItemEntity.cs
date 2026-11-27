using System.Collections.Generic;
using FastEnumUtility;
using GameOff2026.Game.Application;

namespace GameOff2026.Game.Data.Entity
{
    public sealed class PurchasedItemEntity
    {
        private readonly Dictionary<int, PurchasedItemVO> _purchasedItemMap;

        public PurchasedItemEntity()
        {
            _purchasedItemMap = new Dictionary<int, PurchasedItemVO>();
        }

        public PurchasedItemVO Get(int id)
        {
            return _purchasedItemMap.TryGetValue(id, out var purchasedItemVO)
                ? purchasedItemVO
                : new PurchasedItemVO(id, 0);
        }

        public PurchasedItemVO Get(ShopItemId id)
        {
            return Get(id.ToInt32());
        }

        public bool Has(ShopItemId id)
        {
            return Get(id).count > 0;
        }

        public void Add(int id)
        {
            if (_purchasedItemMap.TryGetValue(id, out var item))
            {
                _purchasedItemMap[id] = new PurchasedItemVO(id, item.count + 1);
            }
            else
            {
                _purchasedItemMap[id] = new PurchasedItemVO(id, 1);
            }
        }
    }
}