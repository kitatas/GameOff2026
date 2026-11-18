using Cysharp.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class MoneyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI money = default;

        public Tween Render(int prev, int current, float duration)
        {
            return DOTween.To(
                    () => prev,
                    x => money.text = ZString.Format("{0:N0}", x),
                    current,
                    duration
                )
                .SetLink(gameObject);
        }
    }
}