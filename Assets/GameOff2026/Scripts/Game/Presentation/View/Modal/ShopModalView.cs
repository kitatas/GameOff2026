using DG.Tweening;
using GameOff2026.Common.Presentation.View.Button;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View.Modal
{
    public sealed class ShopModalView : BaseGameModalView
    {
        [SerializeField] private CommonButtonView button = default;

        public override Tween FadeIn(float duration)
        {
            base.FadeIn(duration);

            // click待機
            var tween = DOTween
                .To(() => 0.0f, _ => { }, 1.0f, duration)
                .Pause();
            button.onClick.AddListener(Click);
            return tween;

            void Click()
            {
                tween.Complete();
                button.onClick.RemoveListener(Click);
            }
        }
    }
}