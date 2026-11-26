using GameOff2026.Common.Application;
using GameOff2026.Common.Presentation.View;

namespace GameOff2026.Common.Presentation.Facade
{
    public sealed class BgmFacade
    {
        private readonly ISoundView _soundView;

        public BgmFacade(ISoundView soundView)
        {
            _soundView = soundView;
        }

        public void Play(BgmSoundVO sound)
        {
            _soundView.PlayBgm(sound);
        }

        public void Stop()
        {
            _soundView.StopBgm();
        }

        public void SetVolume(float volume)
        {
            _soundView.SetBgmVolume(volume);
        }

        public void SetMute(bool value)
        {
            if (value)
            {
                _soundView.PauseBgm();
            }
            else
            {
                _soundView.UnPauseBgm();
            }
        }
    }
}