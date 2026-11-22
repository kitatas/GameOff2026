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
                if (enemySquare == null) continue;

                var routeSquares = enemySquare.Arounds()
                    .AsValueEnumerable()
                    .Where(x => _fieldEntity.IsEnemyRouteActive(x))
                    .ToArray();

                if (routeSquares.Length == 0) continue;

                // Route to Enemy
                var square = routeSquares.GetRandom();
                _enemyEntity.Set(_dateEntity.value, i, square);
                Replace(square, _dateEntity.value.ToEnemy(), CalcDirection(square, enemySquare));

                // Enemy to Route
                Replace(enemySquare, FieldType.EnemyRouteActive, Direction.None);
            }
        }

        private void Replace(SquareVO square, FieldType type, Direction direction)
        {
            var (fieldSquare, index) = _fieldEntity.Find(square);
            var field = _fieldRepository.Find(type);
            _fieldEntity.SetField(index, field, direction);
        }

        private static Direction CalcDirection(SquareVO square, SquareVO enemySquare)
        {
            var dir = new SquareVO(square.file - enemySquare.file, square.rank - enemySquare.rank);
            if (dir.IsEqual(Direction.Up.ToSquareVO())) return Direction.Up;
            if (dir.IsEqual(Direction.Down.ToSquareVO())) return Direction.Down;
            if (dir.IsEqual(Direction.Left.ToSquareVO())) return Direction.Left;
            if (dir.IsEqual(Direction.Right.ToSquareVO())) return Direction.Right;
            return Direction.None;
        }
    }
}