using Cysharp.Text;
using TMPro;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class DayView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI day = default;

        public void Render(int value)
        {
            day.text = ZString.Format("{0}", value);
        }
    }
}