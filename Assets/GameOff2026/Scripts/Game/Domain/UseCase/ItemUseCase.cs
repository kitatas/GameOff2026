using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class ItemUseCase
    {
        private readonly MaxStaminaEntity _maxStaminaEntity;

        public ItemUseCase(MaxStaminaEntity maxStaminaEntity)
        {
            _maxStaminaEntity = maxStaminaEntity;
        }

        public void ApplyEffect(ShopItemVO shopItem)
        {
            switch (shopItem.id.ToShopItemId())
            {
                case ShopItemId.MaxStamina:
                    _maxStaminaEntity.Add(shopItem.effectValue);
                    break;
                default:
                    throw new QuitExceptionVO(ExceptionConfig.INVALID_SHOP_ITEM_ID);
            }
        }
    }
}