using System;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class TreasurePresenter : IStartable, IDisposable
    {
        private readonly TreasureUseCase _treasureUseCase;
        private readonly TreasureFacade _treasureFacade;
        private readonly CompositeDisposable _disposable;

        public TreasurePresenter(TreasureUseCase treasureUseCase, TreasureFacade treasureFacade)
        {
            _treasureUseCase = treasureUseCase;
            _treasureFacade = treasureFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _treasureUseCase.treasure
                .DistinctUntilChanged()
                .Pairwise()
                .Subscribe(_treasureFacade.Render)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}