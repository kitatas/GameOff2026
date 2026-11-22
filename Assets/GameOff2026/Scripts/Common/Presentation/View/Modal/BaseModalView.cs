using System;
using DG.Tweening;
using UniEx;
using UnityEngine;
using UnityEngine.UI;

namespace GameOff2026.Common.Presentation.View.Modal
{
    public abstract class BaseModalView<T> : MonoBehaviour where T : Enum
    {
        [SerializeField] private T modalType = default;
        [SerializeField] private CanvasGroup canvasGroup = default;
        [SerializeField] private ScrollRect scrollRect = default;
        private Sequence _tween;

        public T type => modalType;

        private void Awake()
        {
            FadeOut(0.0f);
        }

        public virtual Tween FadeIn(float duration)
        {
            _tween?.Kill();
            PreFadeIn();
            return _tween = DOTween.Sequence()
                .AppendCallback(() => canvasGroup.blocksRaycasts = true)
                .Append(canvasGroup
                    .DOFade(1.0f, duration)
                    .SetEase(Ease.OutBack))
                .Join(canvasGroup.transform.ToRectTransform()
                    .DOScale(Vector3.one, duration)
                    .SetEase(Ease.OutBack))
                .OnComplete(PostFadeIn)
                .SetLink(gameObject);
        }

        public virtual Tween FadeOut(float duration)
        {
            _tween?.Kill();
            PreFadeOut();
            return _tween = DOTween.Sequence()
                .Append(canvasGroup
                    .DOFade(0.0f, duration)
                    .SetEase(Ease.OutQuart))
                .Join(canvasGroup.transform.ToRectTransform()
                    .DOScale(Vector3.one * 0.8f, duration)
                    .SetEase(Ease.OutQuart))
                .AppendCallback(() => canvasGroup.blocksRaycasts = false)
                .OnComplete(() =>
                {
                    if (scrollRect) scrollRect.verticalNormalizedPosition = 1.0f;
                    PostFadeOut();
                })
                .SetLink(gameObject);
        }

        protected virtual void PreFadeIn()
        {
        }

        protected virtual void PostFadeIn()
        {
        }

        protected virtual void PreFadeOut()
        {
        }

        protected virtual void PostFadeOut()
        {
        }
    }
}