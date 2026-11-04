using DG.Tweening;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class SquareView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer = default;

        public Tween Render(Sprite sprite)
        {
            return DOTween.Sequence()
                .AppendCallback(() => spriteRenderer.sprite = sprite)
                .SetLink(gameObject);
        }
    }
}