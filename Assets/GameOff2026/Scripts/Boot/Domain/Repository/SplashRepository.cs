using System.Collections.Generic;
using System.Linq;
using GameOff2026.Boot.Application;
using GameOff2026.Boot.Data.DataStore;
using GameOff2026.Common.Application;

namespace GameOff2026.Boot.Domain.Repository
{
    public sealed class SplashRepository
    {
        private readonly Dictionary<SplashType, SplashVO> _splashMap;

        public SplashRepository(SplashTable splashTable)
        {
            _splashMap = splashTable.all.ToDictionary(x => x.type, x => x.ToVO());
        }

        public SplashVO Find(SplashType type)
        {
            return _splashMap.TryGetValue(type, out var vo)
                ? vo
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_SPLASH);
        }
    }
}