using System;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class StaminaPresenter : IStartable, IDisposable
    {
        private readonly StaminaUseCase _staminaUseCase;
        private readonly StaminaFacade _staminaFacade;
        private readonly CompositeDisposable _disposable;

        public StaminaPresenter(StaminaUseCase staminaUseCase, StaminaFacade staminaFacade)
        {
            _staminaUseCase = staminaUseCase;
            _staminaFacade = staminaFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _staminaUseCase.stamina
                .DistinctUntilChanged()
                .Pairwise()
                .Subscribe(_staminaFacade.Render)
                .AddTo(_disposable);

            _staminaUseCase.maxStamina
                .DistinctUntilChanged()
                .Pairwise()
                .Subscribe(_staminaFacade.RenderMax)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}