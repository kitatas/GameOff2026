using System.Collections.Generic;
using System.Linq;
using GameOff2026.Game.Application;
using GameOff2026.Game.Presentation.View.Button;
using R3;
using UnityEngine;
using ZLinq;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class MovementView : MonoBehaviour
    {
        [SerializeField] private MoveButtonView[] moveButtonViews;

        private Dictionary<Direction, MoveButtonView> _moveButtonMap;

        private Dictionary<Direction, MoveButtonView> moveButtonMap =>
            _moveButtonMap ??= moveButtonViews
                .AsValueEnumerable()
                .ToDictionary(x => x.dir, x => x);

        public void SetInteractableAll(MovableDirectionVO[] movableDirections)
        {
            foreach (var movableDirection in movableDirections)
            {
                moveButtonMap[movableDirection.direction].SetInteractable(movableDirection.isMovable);
            }
        }

        public Observable<Direction> OnClickAnyDirectionAsObservable()
        {
            return moveButtonViews
                .Select(x => x.OnClickDirectionAsObservable())
                .Merge();
        }
    }
}