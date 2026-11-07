using DG.Tweening;
using GameOff2026.Game.Application;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class DoorView : MonoBehaviour
    {
        [SerializeField] private SquareView[] squareViews = default;

        public Tween Render(FieldSquareVO[] squares)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < squareViews.Length; i++)
            {
                var index = FieldConfig.MAX_FILE * FieldConfig.MAX_RANK + i;
                sequence.Join(squareViews[i].Render(squares[index].field.sprite));
            }

            return sequence;
        }
    }
}