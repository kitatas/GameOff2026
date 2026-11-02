using System;
using GameOff2026.Common.Application;
using R3;

namespace GameOff2026.Common.Domain.UseCase
{
    public sealed class SceneUseCase : IDisposable
    {
        private readonly Subject<LoadVO> _load;

        public SceneUseCase()
        {
            _load = new Subject<LoadVO>();
        }

        public Observable<LoadVO> load => _load;

        public void LoadFade(SceneName sceneName, Fade fade)
        {
            _load?.OnNext(LoadVO.CreateFade(sceneName, fade, SceneConfig.FADE_DURATION));
        }

        void IDisposable.Dispose()
        {
            _load?.Dispose();
        }
    }
}