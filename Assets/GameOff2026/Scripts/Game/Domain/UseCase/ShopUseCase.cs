using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.Repository;
using MessagePipe;
using ZLinq;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class ShopUseCase
    {
        private readonly PurchasedItemEntity _purchasedItemEntity;
        private readonly ShopPorts _shopPorts;
        private readonly ShopItemRepository _shopItemRepository;

        public ShopUseCase(PurchasedItemEntity purchasedItemEntity, ShopPorts shopPorts,
            ShopItemRepository shopItemRepository)
        {
            _purchasedItemEntity = purchasedItemEntity;
            _shopPorts = shopPorts;
            _shopItemRepository = shopItemRepository;
        }

        public IAsyncSubscriber<ShopVO[]> shops => _shopPorts.shopSubscriber;

        public UniTask PublishShopsAsync(CancellationToken token)
        {
            var vos = _shopItemRepository.GetAll()
                .AsValueEnumerable()
                .Select(x => new ShopVO(x, _purchasedItemEntity.Get(x.id)))
                .ToArray();

            return _shopPorts.PublishShopsAsync(vos, token);
        }

        public void Buy(int id)
        {
            _purchasedItemEntity.Add(id);
        }
    }
}