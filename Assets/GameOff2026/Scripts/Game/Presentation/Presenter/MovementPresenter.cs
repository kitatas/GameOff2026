using System;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using MessagePipe;
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
            _movementUseCase.movableDirections
                .Subscribe(_movementFacade.RenderButtonsAsync)
                .AddTo(_disposable);

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