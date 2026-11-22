using Cysharp.Text;
using GameOff2026.Game.Application;
using TMPro;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class ShopItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemName = default;
        [SerializeField] private TextMeshProUGUI itemDescription = default;
        [SerializeField] private TextMeshProUGUI price = default;
        [SerializeField] private GameObject uniqLabel = default;

        public void Render(ShopVO shop)
        {
            itemName.text = shop.shopItem.name;
            itemDescription.text = shop.shopItem.description;
            price.text = ZString.Format("${0}", shop.shopItem.price);
            uniqLabel.SetActive(shop.shopItem.isUniq);
        }
    }
}