using DG.Tweening;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class PlayerView : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform = default;

        public Tween Move(Vector2 position, float duration)
        {
            return playerTransform
                .DOLocalMove(position, duration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }
    }
}