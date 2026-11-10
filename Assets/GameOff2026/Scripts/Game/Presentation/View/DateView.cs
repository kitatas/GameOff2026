using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class DateView : MonoBehaviour
    {
        [SerializeField] private Image mask = default;

        public Tween RenderDay(float duration)
        {
            return mask
                .DOFade(0.0f, duration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        public Tween RenderNight(float duration)
        {
            return mask
                .DOFade(0.5f, duration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }
    }
}