using System;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class ShopPresenter : IStartable, IDisposable
    {
        private readonly ShopUseCase _shopUseCase;
        private readonly ShopFacade _shopFacade;
        private readonly CompositeDisposable _disposable;

        public ShopPresenter(ShopUseCase shopUseCase, ShopFacade shopFacade)
        {
            _shopUseCase = shopUseCase;
            _shopFacade = shopFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _shopUseCase.shops
                .Subscribe(_shopFacade.RenderAsync)
                .AddTo(_disposable);

            _shopFacade.OnClickBuyAsObservable()
                .Subscribe(_shopUseCase.Buy)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}