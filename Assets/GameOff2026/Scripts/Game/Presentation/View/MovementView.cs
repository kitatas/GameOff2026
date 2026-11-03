using System.Linq;
using GameOff2026.Game.Application;
using GameOff2026.Game.Presentation.View.Button;
using R3;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View
{
    public sealed class MovementView : MonoBehaviour
    {
        [SerializeField] private MoveButtonView[] moveButtonViews;

        public Observable<Direction> OnClickAnyDirectionAsObservable()
        {
            return moveButtonViews
                .Select(x => x.OnClickDirectionAsObservable())
                .Merge();
        }
    }
}