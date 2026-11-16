using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameWanderState : BaseGameState
    {
        private readonly EnemyWanderUseCase _enemyWanderUseCase;
        private readonly FieldUseCase _fieldUseCase;
        private readonly PlayerUseCase _playerUseCase;

        public GameWanderState(EnemyWanderUseCase enemyWanderUseCase, FieldUseCase fieldUseCase,
            PlayerUseCase playerUseCase)
        {
            _enemyWanderUseCase = enemyWanderUseCase;
            _fieldUseCase = fieldUseCase;
            _playerUseCase = playerUseCase;
        }

        public override GameState state => GameState.Wander;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            _enemyWanderUseCase.Wander();
            await _fieldUseCase.PublishRenderAsync(token);

            return _playerUseCase.IsEncounterEnemy()
                ? GameState.Fail
                : GameState.Input;
        }
    }
}