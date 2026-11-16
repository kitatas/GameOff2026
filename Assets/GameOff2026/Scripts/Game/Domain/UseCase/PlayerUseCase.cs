using GameOff2026.Game.Data.Entity;
using ZLinq;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class PlayerUseCase
    {
        private readonly DateEntity _dateEntity;
        private readonly EnemyEntity _enemyEntity;
        private readonly PlayerEntity _playerEntity;

        public PlayerUseCase(DateEntity dateEntity, EnemyEntity enemyEntity, PlayerEntity playerEntity)
        {
            _dateEntity = dateEntity;
            _enemyEntity = enemyEntity;
            _playerEntity = playerEntity;
        }

        public bool IsEncounterEnemy()
        {
            return _enemyEntity.Get(_dateEntity.value)
                .AsValueEnumerable()
                .Any(x => _playerEntity.square.IsEqual(x));
        }
    }
}