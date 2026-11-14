using System.Collections.Generic;
using GameOff2026.Game.Application;

namespace GameOff2026.Game.Data.Entity
{
    public sealed class EnemyEntity
    {
        private readonly Dictionary<DateType, SquareVO[]> _enemyMap;

        public EnemyEntity()
        {
            _enemyMap = new Dictionary<DateType, SquareVO[]>
            {
                { DateType.Day, new SquareVO[4] },
                { DateType.Night, new SquareVO[4] },
            };
        }

        public void Set(DateType date, int index, SquareVO square)
        {
            _enemyMap[date][index] = square;
        }
    }
}