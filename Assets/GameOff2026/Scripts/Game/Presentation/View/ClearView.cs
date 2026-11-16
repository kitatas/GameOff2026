using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class ClearView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI clear = default;

        public Tween FadeIn(float duration)
        {
            return clear
                .DOFade(1.0f, duration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            return clear
                .DOFade(0.0f, duration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }
    }
}