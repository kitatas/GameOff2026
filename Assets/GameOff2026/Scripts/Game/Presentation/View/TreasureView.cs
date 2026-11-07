using Cysharp.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class TreasureView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI treasureCount = default;

        public Tween Render(int prev, int current, float duration)
        {
            return DOTween.To(
                    () => prev,
                    x => treasureCount.text = ZString.Format("{0}", x),
                    current,
                    duration
                )
                .SetLink(gameObject);
        }
    }
}