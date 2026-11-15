using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Repository;
using UniEx;
using ZLinq;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class EnemyWanderUseCase
    {
        private readonly DateEntity _dateEntity;
        private readonly EnemyEntity _enemyEntity;
        private readonly FieldEntity _fieldEntity;
        private readonly FieldRepository _fieldRepository;

        public EnemyWanderUseCase(DateEntity dateEntity, EnemyEntity enemyEntity, FieldEntity fieldEntity,
            FieldRepository fieldRepository)
        {
            _dateEntity = dateEntity;
            _enemyEntity = enemyEntity;
            _fieldEntity = fieldEntity;
            _fieldRepository = fieldRepository;
        }

        public void Wander()
        {
            var enemies = _enemyEntity.Get(_dateEntity.value);
            for (int i = 0; i < enemies.Length; i++)
            {
                var enemySquare = enemies[i];
                var routeSquares = enemySquare.Arounds()
                    .AsValueEnumerable()
                    .Where(x => _fieldEntity.IsEnemyRouteActive(x))
                    .ToArray();

                if (routeSquares.Length == 0) continue;

                // Route to Enemy
                var square = routeSquares.GetRandom();
                _enemyEntity.Set(_dateEntity.value, i, square);
                Replace(square, _dateEntity.value.ToEnemy());

                // Enemy to Route
                Replace(enemySquare, FieldType.EnemyRouteActive);
            }
        }

        private void Replace(SquareVO square, FieldType type)
        {
            var (fieldSquare, index) = _fieldEntity.Find(square);
            var field = _fieldRepository.Find(type);
            _fieldEntity.SetField(index, field);
        }
    }
}