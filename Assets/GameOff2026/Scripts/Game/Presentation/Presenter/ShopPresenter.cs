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
        private readonly MoneyUseCase _moneyUseCase;
        private readonly ShopUseCase _shopUseCase;
        private readonly ShopFacade _shopFacade;
        private readonly CompositeDisposable _disposable;

        public ShopPresenter(MoneyUseCase moneyUseCase, ShopUseCase shopUseCase, ShopFacade shopFacade)
        {
            _moneyUseCase = moneyUseCase;
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
                .Subscribe(x =>
                {
                    var shopItem = _shopUseCase.Buy(x);
                    _moneyUseCase.Pay(shopItem.price);
                })
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}