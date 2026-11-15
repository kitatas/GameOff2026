using Cysharp.Text;
using GameOff2026.Game.Application;
using TMPro;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class DayView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI day = default;
        [SerializeField] private TextMeshProUGUI maxDay = default;

        private void Start()
        {
            maxDay.text = ZString.Format("{0}", DayConfig.MAX_DAY);
        }

        public void Render(int value)
        {
            day.text = ZString.Format("{0}", value);
        }
    }
}