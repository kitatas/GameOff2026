using System;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class FieldPresenter : IStartable, IDisposable
    {
        private readonly FieldUseCase _fieldUseCase;
        private readonly FieldFacade _fieldFacade;
        private readonly CompositeDisposable _disposable;

        public FieldPresenter(FieldUseCase fieldUseCase, FieldFacade fieldFacade)
        {
            _fieldUseCase = fieldUseCase;
            _fieldFacade = fieldFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _fieldUseCase.renderField
                .Subscribe(_fieldFacade.RenderAsync)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}