using System;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class HudRootPresenter : IStartable, IDisposable
    {
        private readonly HudRootUseCase _hudRootUseCase;
        private readonly HudRootFacade _hudRootFacade;
        private readonly CompositeDisposable _disposable;

        public HudRootPresenter(HudRootUseCase hudRootUseCase, HudRootFacade hudRootFacade)
        {
            _hudRootUseCase = hudRootUseCase;
            _hudRootFacade = hudRootFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _hudRootUseCase.transition
                .Subscribe(_hudRootFacade.FadeAsync)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}