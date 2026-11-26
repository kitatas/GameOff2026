using System;
using GameOff2026.Common.Application;

namespace GameOff2026.Common.Domain.Repository
{
    public interface ISoundRepository
    {
        BgmAudioVO Find(BgmType type);
        SeAudioVO Find(SeType type);
        AudioVO<T> Find<T>(T type) where T : Enum;
    }
}