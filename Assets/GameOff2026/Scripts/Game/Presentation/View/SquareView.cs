using DG.Tweening;
using GameOff2026.Game.Application;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class SquareView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer = default;

        public Tween Render(FieldSquareVO fieldSquare)
        {
            var sequence = DOTween.Sequence()
                .AppendCallback(() => spriteRenderer.sprite = fieldSquare.field.sprite);

            if (fieldSquare.direction is not Direction.None)
            {
                var currentSquare = transform.position;
                var direction = fieldSquare.direction.ToSquareVO();
                var startSquare = new Vector2(
                    currentSquare.x + (direction.file * -1),
                    currentSquare.y + (direction.rank * -1));

                sequence
                    .Append(transform
                        .DOMove(startSquare, 0.0f)
                        .SetEase(Ease.Linear))
                    .Append(transform
                        .DOMove(currentSquare, PlayerConfig.MOVE_DURATION)
                        .SetEase(Ease.Linear));
            }

            return sequence
                .SetLink(gameObject);
        }
    }
}