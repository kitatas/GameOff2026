using DG.Tweening;
using GameOff2026.Common.Application;
using TMPro;
using UnityEngine;

namespace GameOff2026.Common.Presentation.View
{
    public sealed class LoadingView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI loading = default;
        [SerializeField] private CanvasGroup canvasGroup = default;

        private Sequence _tween;

        private void Awake()
        {
            _tween = DOTween.Sequence()
                .Join(TweenLoadingText())
                .SetLoops(-1)
                .SetLink(gameObject);
        }

        private Tween TweenLoadingText()
        {
            var animator = new DOTweenTMPAnimator(loading);
            var offset = Vector3.up * 5.0f;
            var length = loading.textInfo.characterCount;

            var sequence = DOTween.Sequence();
            for (int i = 0; i < length; i++)
            {
                if (char.IsWhiteSpace(loading.text[i])) continue;

                var interval = (i + 1) * LoadingConfig.TEXT_ANIMATION_INTERVAL;
                var endValue = animator.GetCharOffset(i) + offset;
                sequence
                    .Join(DOTween.Sequence()
                        .AppendInterval(interval)
                        .Append(animator
                            .DOOffsetChar(i, endValue, LoadingConfig.TEXT_ANIMATION_DURATION)
                            .SetLoops(2, LoopType.Yoyo)
                            .SetDelay(interval))
                        .AppendInterval(length * LoadingConfig.TEXT_ANIMATION_INTERVAL - interval));
            }

            return sequence
                .SetLink(gameObject);
        }

        public Tween FadeIn(float duration)
        {
            _tween?.Restart();
            return DOTween.Sequence()
                .AppendCallback(() => canvasGroup.blocksRaycasts = true)
                .Append(canvasGroup
                    .DOFade(1.0f, duration)
                    .SetEase(Ease.OutBack))
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            _tween?.Pause();
            return DOTween.Sequence()
                .Append(canvasGroup
                    .DOFade(0.0f, duration)
                    .SetEase(Ease.OutQuart))
                .AppendCallback(() => canvasGroup.blocksRaycasts = false)
                .SetLink(gameObject);
        }
    }
}