using System.Threading;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Domain.Repository;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.Repository;
using MessagePipe;
using UnityEngine;
using ZLinq;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class ShopUseCase
    {
        private readonly MoneyEntity _moneyEntity;
        private readonly PurchasedItemEntity _purchasedItemEntity;
        private readonly ShopPriceRateEntity _shopPriceRateEntity;
        private readonly ShopPorts _shopPorts;
        private readonly LocaleRepository _localeRepository;
        private readonly ShopItemRepository _shopItemRepository;

        public ShopUseCase(MoneyEntity moneyEntity, PurchasedItemEntity purchasedItemEntity,
            ShopPriceRateEntity shopPriceRateEntity, ShopPorts shopPorts, LocaleRepository localeRepository,
            ShopItemRepository shopItemRepository)
        {
            _moneyEntity = moneyEntity;
            _purchasedItemEntity = purchasedItemEntity;
            _shopPriceRateEntity = shopPriceRateEntity;
            _shopPorts = shopPorts;
            _localeRepository = localeRepository;
            _shopItemRepository = shopItemRepository;
        }

        public IAsyncSubscriber<ShopVO[]> shops => _shopPorts.shopSubscriber;

        public UniTask PublishShopsAsync(CancellationToken token)
        {
            var vos = _shopItemRepository.GetAll()
                .AsValueEnumerable()
                .Select(x =>
                {
                    var name = _localeRepository.Get(x.name);
                    var description = ZString.Format(_localeRepository.Get(x.description), x.effectValue);
                    var price = Mathf.CeilToInt(x.price * _shopPriceRateEntity.rateValue);
                    var shopItem = ShopItemVO.Create(x, name, description, price);
                    return new ShopVO(shopItem, _purchasedItemEntity.Get(x.id), _moneyEntity.IsEnough(shopItem.price));
                })
                .ToArray();

            return _shopPorts.PublishShopsAsync(vos, token);
        }

        public ShopItemVO Buy(int id)
        {
            _purchasedItemEntity.Add(id);
            return _shopItemRepository.Find(id);
        }
    }
}