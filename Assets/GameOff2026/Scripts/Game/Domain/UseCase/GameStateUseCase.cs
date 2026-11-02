using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Data.Entity;
using GameOff2026.Common.Domain.UseCase;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class GameStateUseCase : BaseStateUseCase<GameState>
    {
        public GameStateUseCase(GameStateEntity entity, RetryCountEntity retryCountEntity, GameStatePorts ports) : base(
            entity, retryCountEntity, ports)
        {
        }

        public override UniTask InitAsync(CancellationToken token)
        {
            return PublishAsync(GameState.Init, token);
        }

        public override UniTask PublishAsync(GameState value, CancellationToken token)
        {
            return value is GameState.None
                ? UniTask.Yield(token)
                : base.PublishAsync(value, token);
        }
    }
}