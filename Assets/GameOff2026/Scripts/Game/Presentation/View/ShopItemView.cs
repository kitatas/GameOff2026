using Cysharp.Text;
using GameOff2026.Common.Presentation.View.Button;
using GameOff2026.Common.Utility;
using GameOff2026.Game.Application;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class ShopItemView : MonoBehaviour
    {
        [SerializeField] private Image icon = default;
        [SerializeField] private TextMeshProUGUI itemName = default;
        [SerializeField] private TextMeshProUGUI itemDescription = default;
        [SerializeField] private TextMeshProUGUI price = default;
        [SerializeField] private GameObject uniqLabel = default;
        [SerializeField] private CommonButtonView button = default;
        private int _id;

        public void Render(ShopVO shop)
        {
            var path = ZString.Format(ShopItemConfig.IMG_PATH, shop.shopItem.id);
            this.LoadAsset<Sprite>(path, x =>
            {
                _id = shop.shopItem.id;
                icon.sprite = x;
                itemName.text = shop.shopItem.name;
                itemDescription.text = shop.shopItem.description;
                price.text = shop.isSoldOut
                    ? $"sold\nout"
                    : ZString.Format("${0}", shop.shopItem.price);
                uniqLabel.SetActive(shop.shopItem.isUniq);
                button.SetInteractable(shop.canBuy);
            });
        }

        public Observable<int> OnClickBuyAsObservable()
        {
            return button.click.Select(_ => _id);
        }
    }
}