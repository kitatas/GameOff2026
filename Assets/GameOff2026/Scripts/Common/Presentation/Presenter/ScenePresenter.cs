using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.UseCase;
using GameOff2026.Common.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace GameOff2026.Common.Presentation.Presenter
{
    public sealed class ScenePresenter : IInitializable, IDisposable
    {
        private readonly SceneUseCase _sceneUseCase;
        private readonly SceneFacade _sceneFacade;
        private readonly CompositeDisposable _disposable;

        public ScenePresenter(SceneUseCase sceneUseCase, SceneFacade sceneFacade)
        {
            _sceneUseCase = sceneUseCase;
            _sceneFacade = sceneFacade;
            _disposable = new CompositeDisposable();
        }

        void IInitializable.Initialize()
        {
            _sceneUseCase.load
                .SubscribeAwait(async (x, ct) =>
                {
                    if (x.loadType is LoadType.Fade) await FadeInAsync(x.transition, ct);
                    await _sceneFacade.LoadSceneAsync(x.sceneName, ct);
                    if (x.loadType is LoadType.Fade) await FadeOutAsync(x.transition, ct);
                })
                .AddTo(_disposable);
        }

        private async UniTask FadeInAsync(TransitionVO transition, CancellationToken token)
        {
            var t = new TransitionVO(Fade.In, transition.duration);
            await _sceneFacade.FadeAsync(t, token);
        }

        private async UniTask FadeOutAsync(TransitionVO transition, CancellationToken token)
        {
            var t = new TransitionVO(Fade.Out, transition.duration);
            await _sceneFacade.FadeAsync(t, token);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}