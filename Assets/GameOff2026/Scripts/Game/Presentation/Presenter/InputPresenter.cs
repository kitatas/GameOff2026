using System;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class InputPresenter : IStartable, IDisposable
    {
        private readonly InputUseCase _inputUseCase;
        private readonly InputFacade _inputFacade;
        private readonly CompositeDisposable _disposable;

        public InputPresenter(InputUseCase inputUseCase, InputFacade inputFacade)
        {
            _inputUseCase = inputUseCase;
            _inputFacade = inputFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _inputFacade.InputAnyAsObservable()
                .Subscribe(_inputUseCase.HandleInput)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}