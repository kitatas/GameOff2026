using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class FailView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI fail = default;

        public Tween FadeIn(float duration)
        {
            return fail
                .DOFade(1.0f, duration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            return fail
                .DOFade(0.0f, duration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }
    }
}