using Cysharp.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class DateView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI date = default;
        [SerializeField] private Image mask = default;

        public Tween RenderDay(float duration)
        {
            return DOTween.Sequence()
                .Append(mask
                    .DOFade(0.0f, duration)
                    .SetEase(Ease.Linear))
                .AppendCallback(() => date.text = ZString.Format("{0}", "Day"))
                .SetLink(gameObject);
        }

        public Tween RenderNight(float duration)
        {
            return DOTween.Sequence()
                .Append(mask
                    .DOFade(0.5f, duration)
                    .SetEase(Ease.Linear))
                .AppendCallback(() => date.text = ZString.Format("{0}", "Night"))
                .SetLink(gameObject);
        }
    }
}