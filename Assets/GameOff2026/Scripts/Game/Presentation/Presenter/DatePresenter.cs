using System;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class DatePresenter : IStartable, IDisposable
    {
        private readonly DateUseCase _dateUseCase;
        private readonly DateFacade _dateFacade;
        private readonly CompositeDisposable _disposable;

        public DatePresenter(DateUseCase dateUseCase, DateFacade dateFacade)
        {
            _dateUseCase = dateUseCase;
            _dateFacade = dateFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _dateUseCase.dateTransition
                .Subscribe(_dateFacade.RenderAsync)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}