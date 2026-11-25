using System;
using GameOff2026.Common.Domain.UseCase;
using GameOff2026.Common.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace GameOff2026.Common.Presentation.Presenter
{
    public sealed class LoadingPresenter : IInitializable, IDisposable
    {
        private readonly LoadingUseCase _loadingUseCase;
        private readonly LoadingFacade _loadingFacade;
        private readonly CompositeDisposable _disposable;

        public LoadingPresenter(LoadingUseCase loadingUseCase, LoadingFacade loadingFacade)
        {
            _loadingUseCase = loadingUseCase;
            _loadingFacade = loadingFacade;
            _disposable = new CompositeDisposable();
        }

        void IInitializable.Initialize()
        {
            _loadingUseCase.transition
                .Subscribe(_loadingFacade.FadeAsync)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}