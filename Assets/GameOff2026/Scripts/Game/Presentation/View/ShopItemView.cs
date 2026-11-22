using Cysharp.Text;
using GameOff2026.Common.Presentation.View.Button;
using GameOff2026.Game.Application;
using R3;
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
        [SerializeField] private CommonButtonView button = default;
        private int _id;

        public void Render(ShopVO shop)
        {
            _id = shop.shopItem.id;
            itemName.text = shop.shopItem.name;
            itemDescription.text = shop.shopItem.description;
            price.text = ZString.Format("${0}", shop.shopItem.price);
            uniqLabel.SetActive(shop.shopItem.isUniq);

            // 在庫があり、所持金が足りているか
            var canBuy = !shop.shopItem.isUniq || shop.purchasedItem.count == 0;
            canBuy = canBuy && shop.isEnoughMoney;
            button.SetInteractable(canBuy);
        }

        public Observable<int> OnClickBuyAsObservable()
        {
            return button.click.Select(_ => _id);
        }
    }
}