using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class ShopPresenter : IStartable, IDisposable
    {
        private readonly ItemUseCase _itemUseCase;
        private readonly MoneyUseCase _moneyUseCase;
        private readonly ShopUseCase _shopUseCase;
        private readonly ShopFacade _shopFacade;
        private readonly CancellationTokenSource _tokenSource;
        private readonly CompositeDisposable _disposable;

        public ShopPresenter(ItemUseCase itemUseCase, MoneyUseCase moneyUseCase, ShopUseCase shopUseCase,
            ShopFacade shopFacade)
        {
            _itemUseCase = itemUseCase;
            _moneyUseCase = moneyUseCase;
            _shopUseCase = shopUseCase;
            _shopFacade = shopFacade;
            _tokenSource = new CancellationTokenSource();
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
                    _itemUseCase.ApplyEffect(shopItem);
                    _moneyUseCase.Pay(shopItem.price);
                    _shopUseCase.PublishShopsAsync(_tokenSource.Token).Forget();
                })
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            _disposable?.Dispose();
        }
    }
}