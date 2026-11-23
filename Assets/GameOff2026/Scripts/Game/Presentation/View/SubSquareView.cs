using DG.Tweening;
using GameOff2026.Game.Application;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class SubSquareView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer = default;
        [SerializeField] private Sprite enemyRouteActiveSprite = default;

        public void Render(FieldType type)
        {
            spriteRenderer.sprite = type.IsEnemy()
                ? enemyRouteActiveSprite
                : null;
        }

        public Tween LocalMove(Vector2 target, float duration)
        {
            return spriteRenderer.transform
                .DOLocalMove(target, duration);
        }
    }
}