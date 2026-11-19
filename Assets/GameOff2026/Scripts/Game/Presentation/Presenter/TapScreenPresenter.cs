using System;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class TapScreenPresenter : IStartable, IDisposable
    {
        private readonly TapScreenUseCase _tapScreenUseCase;
        private readonly TapScreenFacade _tapScreenFacade;
        private readonly CompositeDisposable _disposable;

        public TapScreenPresenter(TapScreenUseCase tapScreenUseCase, TapScreenFacade tapScreenFacade)
        {
            _tapScreenUseCase = tapScreenUseCase;
            _tapScreenFacade = tapScreenFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _tapScreenUseCase.tapScreenTransition
                .Subscribe((t, ct) => _tapScreenFacade.FadeAsync(t, ct))
                .AddTo(_disposable);

            _tapScreenFacade.OnTapAsObservable()
                .Subscribe(_ => _tapScreenUseCase.NotifyTapScreen())
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}