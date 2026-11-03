using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Presentation.View;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class PlayerFacade
    {
        private readonly PlayerView _playerView;

        public PlayerFacade(PlayerView playerView)
        {
            _playerView = playerView;
        }

        public UniTask MoveAsync(MovementVO movement, CancellationToken token)
        {
            return _playerView
                .Move(movement.square.point, movement.duration)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}