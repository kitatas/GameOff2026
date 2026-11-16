using System;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class FinishPresenter : IStartable, IDisposable
    {
        private readonly FinishUseCase _finishUseCase;
        private readonly FinishFacade _finishFacade;
        private readonly CompositeDisposable _disposable;

        public FinishPresenter(FinishUseCase finishUseCase, FinishFacade finishFacade)
        {
            _finishUseCase = finishUseCase;
            _finishFacade = finishFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _finishUseCase.finishTransition
                .Subscribe(_finishFacade.FadeAsync)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}