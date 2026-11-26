using GameOff2026.Common.Application;

namespace GameOff2026.Common.Presentation.View
{
    public interface ISoundView
    {
        void PlayBgm(BgmSoundVO sound);
        void StopBgm();
        void PlaySe(SeSoundVO sound);
        void SetBgmVolume(float volume);
        void SetSeVolume(float volume);
        void PauseBgm();
        void UnPauseBgm();
    }
}