using GameOff2026.Game.Application;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class ShopView : MonoBehaviour
    {
        [SerializeField] private ShopItemView[] shopItemViews = default;

        public void Render(ShopVO[] shops)
        {
            for (int i = 0; i < shops.Length; i++)
            {
                shopItemViews[i].Render(shops[i]);
            }
        }
    }
}