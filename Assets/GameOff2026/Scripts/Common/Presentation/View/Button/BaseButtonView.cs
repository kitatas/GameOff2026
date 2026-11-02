using R3;
using UnityEngine;

namespace GameOff2026.Common.Presentation.View.Button
{
    public abstract class BaseButtonView : MonoBehaviour
    {
        [SerializeField] protected UnityEngine.UI.Button button = default;

        public Observable<Unit> click => button
            .OnClickAsObservable();

        public bool isInitialized { get; private set; } = false;

        public void Init()
        {
            isInitialized = true;
        }

        public void SetInteractable(bool value)
        {
            button.interactable = value;
        }
    }
}