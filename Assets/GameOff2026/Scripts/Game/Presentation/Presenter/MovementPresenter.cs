using System;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class MovementPresenter : IStartable, IDisposable
    {
        private readonly MovementUseCase _movementUseCase;
        private readonly MovementFacade _movementFacade;
        private readonly CompositeDisposable _disposable;

        public MovementPresenter(MovementUseCase movementUseCase, MovementFacade movementFacade)
        {
            _movementUseCase = movementUseCase;
            _movementFacade = movementFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _movementFacade.OnClickAnyDirectionAsObservable()
                .Subscribe(_movementUseCase.HandleClickDirection)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}