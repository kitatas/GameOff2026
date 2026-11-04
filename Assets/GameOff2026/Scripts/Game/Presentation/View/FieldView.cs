using DG.Tweening;
using GameOff2026.Game.Application;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class FieldView : MonoBehaviour
    {
        [SerializeField] private SquareView[] squareViews = default;

        public Tween Render(FieldSquareVO[] squares)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < squareViews.Length; i++)
            {
                sequence.Join(squareViews[i].Render(squares[i].field.sprite));
            }

            return sequence;
        }
    }
}