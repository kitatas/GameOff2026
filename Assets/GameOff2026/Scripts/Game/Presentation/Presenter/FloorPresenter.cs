using System;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class FloorPresenter : IStartable, IDisposable
    {
        private readonly FloorUseCase _floorUseCase;
        private readonly FloorFacade _floorFacade;
        private readonly CompositeDisposable _disposable;

        public FloorPresenter(FloorUseCase floorUseCase, FloorFacade floorFacade)
        {
            _floorUseCase = floorUseCase;
            _floorFacade = floorFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _floorUseCase.floor
                .DistinctUntilChanged()
                .Pairwise()
                .Subscribe(_floorFacade.Render)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}