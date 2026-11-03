using GameOff2026.Common.Presentation.View.Button;
using GameOff2026.Game.Application;
using R3;
using UnityEngine;

namespace GameOff2026.Game.Presentation.View.Button
{
    public sealed class MoveButtonView : BaseButtonView
    {
        [SerializeField] private Direction direction = default;

        public Observable<Direction> OnClickDirectionAsObservable()
        {
            return click.Select(_ => direction);
        }
    }
}