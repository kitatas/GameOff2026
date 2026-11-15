using System;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class DayPresenter : IStartable, IDisposable
    {
        private readonly DayUseCase _dayUseCase;
        private readonly DayFacade _dayFacade;
        private readonly CompositeDisposable _disposable;

        public DayPresenter(DayUseCase dayUseCase, DayFacade dayFacade)
        {
            _dayUseCase = dayUseCase;
            _dayFacade = dayFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _dayUseCase.day
                .Subscribe(_dayFacade.Render)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}