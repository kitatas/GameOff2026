using System;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class MoneyPresenter : IStartable, IDisposable
    {
        private readonly MoneyUseCase _moneyUseCase;
        private readonly MoneyFacade _moneyFacade;
        private readonly CompositeDisposable _disposable;

        public MoneyPresenter(MoneyUseCase moneyUseCase, MoneyFacade moneyFacade)
        {
            _moneyUseCase = moneyUseCase;
            _moneyFacade = moneyFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _moneyUseCase.money
                .DistinctUntilChanged()
                .Pairwise()
                .Subscribe(_moneyFacade.Render)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}