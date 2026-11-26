using GameOff2026.Common.Application;
using GameOff2026.Common.Presentation.View;

namespace GameOff2026.Common.Presentation.Facade
{
    public sealed class SeFacade
    {
        private readonly ISoundView _soundView;

        public SeFacade(ISoundView soundView)
        {
            _soundView = soundView;
        }

        public void Play(SeSoundVO sound)
        {
            _soundView.PlaySe(sound);
        }

        public void SetVolume(float volume)
        {
            _soundView.SetSeVolume(volume);
        }
    }
}