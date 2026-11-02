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
                    if (x.loadType is LoadType.Fade) await FadeInAsync(x, ct);
                    await _sceneFacade.LoadSceneAsync(x.sceneName, ct);
                    if (x.loadType is LoadType.Fade) await FadeOutAsync(x, ct);
                })
                .AddTo(_disposable);
        }

        private async UniTask FadeInAsync(LoadVO load, CancellationToken token)
        {
            await _sceneFacade.FadeAsync(load, token);
        }

        private async UniTask FadeOutAsync(LoadVO load, CancellationToken token)
        {
            await _sceneFacade.FadeAsync(load, token);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}