using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameOff2026.Common.Presentation.View
{
    public sealed class TransitionView : MonoBehaviour
    {
        [SerializeField] private Image blocker = default;

        public Tween FadeIn(float duration)
        {
            return DOTween.Sequence()
                .AppendCallback(() => blocker.raycastTarget = true)
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            return DOTween.Sequence()
                .AppendCallback(() => blocker.raycastTarget = false)
                .SetLink(gameObject);
        }
    }
}