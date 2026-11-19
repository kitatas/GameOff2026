using DG.Tweening;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class HudRootView : MonoBehaviour
    {
        [SerializeField] private RectTransform topSide = default;
        [SerializeField] private RectTransform rightSide = default;
        [SerializeField] private RectTransform leftSide = default;

        public Tween FadeIn(float duration)
        {
            var topY = Mathf.Abs(topSide.anchoredPosition.y) * -1.0f;
            var rightX = Mathf.Abs(rightSide.anchoredPosition.x) * -1.0f;
            var leftX = Mathf.Abs(leftSide.anchoredPosition.x);

            return DOTween.Sequence()
                .Join(topSide
                    .DOAnchorPosY(topY, duration)
                    .SetEase(Ease.OutBack))
                .Join(rightSide
                    .DOAnchorPosX(rightX, duration)
                    .SetEase(Ease.OutBack))
                .Join(leftSide
                    .DOAnchorPosX(leftX, duration)
                    .SetEase(Ease.OutBack))
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            var topY = Mathf.Abs(topSide.anchoredPosition.y);
            var rightX = Mathf.Abs(rightSide.anchoredPosition.x);
            var leftX = Mathf.Abs(leftSide.anchoredPosition.x) * -1.0f;

            return DOTween.Sequence()
                .Join(topSide
                    .DOAnchorPosY(topY, duration)
                    .SetEase(Ease.OutBack))
                .Join(rightSide
                    .DOAnchorPosX(rightX, duration)
                    .SetEase(Ease.OutBack))
                .Join(leftSide
                    .DOAnchorPosX(leftX, duration)
                    .SetEase(Ease.OutBack))
                .SetLink(gameObject);
        }
    }
}