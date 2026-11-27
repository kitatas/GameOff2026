using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class ItemUseCase
    {
        private readonly MaxStaminaEntity _maxStaminaEntity;
        private readonly ShopPriceRateEntity _shopPriceRateEntity;
        private readonly TreasurePriceRateEntity _treasurePriceRateEntity;

        public ItemUseCase(MaxStaminaEntity maxStaminaEntity, ShopPriceRateEntity shopPriceRateEntity,
            TreasurePriceRateEntity treasurePriceRateEntity)
        {
            _maxStaminaEntity = maxStaminaEntity;
            _shopPriceRateEntity = shopPriceRateEntity;
            _treasurePriceRateEntity = treasurePriceRateEntity;
        }

        public void ApplyEffect(ShopItemVO shopItem)
        {
            switch (shopItem.id.ToShopItemId())
            {
                case ShopItemId.MaxStamina:
                    _maxStaminaEntity.Add(shopItem.effectValue);
                    break;
                case ShopItemId.TreasurePriceRate:
                    _treasurePriceRateEntity.Add(shopItem.effectValue);
                    break;
                case ShopItemId.ShopPriceRate:
                    _shopPriceRateEntity.Subtract(shopItem.effectValue);
                    break;
                case ShopItemId.ThroughBlock:
                    break;
                default:
                    throw new QuitExceptionVO(ExceptionConfig.INVALID_SHOP_ITEM_ID);
            }
        }
    }
}