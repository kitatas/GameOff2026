using System;
using GameOff2026.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class MovementPresenter : IStartable, IDisposable
    {
        private readonly MovementFacade _movementFacade;
        private readonly CompositeDisposable _disposable;

        public MovementPresenter(MovementFacade movementFacade)
        {
            _movementFacade = movementFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _movementFacade.OnClickAnyDirectionAsObservable()
                .Subscribe(_ => { /* TODO */ })
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}