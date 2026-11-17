using Cysharp.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class StaminaView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stamina = default;
        [SerializeField] private TextMeshProUGUI maxStamina = default;

        public Tween Render(int prev, int current, float duration)
        {
            return DOTween.To(
                    () => prev,
                    x => stamina.text = ZString.Format("{0:N0}", x),
                    current,
                    duration
                )
                .SetLink(gameObject);
        }

        public Tween RenderMax(int prev, int current, float duration)
        {
            return DOTween.To(
                    () => prev,
                    x => maxStamina.text = ZString.Format("{0:N0}", x),
                    current,
                    duration
                )
                .SetLink(gameObject);
        }
    }
}