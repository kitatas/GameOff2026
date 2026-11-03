using System;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class PlayerPresenter : IStartable, IDisposable
    {
        private readonly MovementUseCase _movementUseCase;
        private readonly PlayerFacade _playerFacade;
        private readonly CompositeDisposable _disposable;

        public PlayerPresenter(MovementUseCase movementUseCase, PlayerFacade playerFacade)
        {
            _movementUseCase = movementUseCase;
            _playerFacade = playerFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _movementUseCase.movement
                .Subscribe(_playerFacade.MoveAsync)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}