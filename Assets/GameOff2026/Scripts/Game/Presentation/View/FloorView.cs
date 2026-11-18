using Cysharp.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class FloorView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI floor = default;

        public Tween Render(int prev, int current, float duration)
        {
            return DOTween.To(
                    () => prev,
                    x => floor.text = ZString.Format("{0:N0}", x),
                    current,
                    duration
                )
                .SetLink(gameObject);
        }
    }
}