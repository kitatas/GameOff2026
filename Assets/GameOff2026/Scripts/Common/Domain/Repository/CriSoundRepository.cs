using System;
using FastEnumUtility;
using GameOff2026.Common.Application;

namespace GameOff2026.Common.Domain.Repository
{
    public sealed class CriSoundRepository : ISoundRepository
    {
        public BgmAudioVO Find(BgmType type)
        {
            return type is BgmType.None
                ? throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_BGM)
                : new BgmAudioVO(type, type.FastToString());
        }

        public SeAudioVO Find(SeType type)
        {
            return type is SeType.None
                ? throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_SE)
                : new SeAudioVO(type, type.FastToString());
        }

        public AudioVO<T> Find<T>(T type) where T : Enum
        {
            return type switch
            {
                BgmType bgm => Find(bgm) as AudioVO<T>,
                SeType se => Find(se) as AudioVO<T>,
                _ => throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_SOUND_TYPE),
            };
        }
    }
}