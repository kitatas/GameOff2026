using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Presentation.View;
using R3;

namespace GameOff2026.Game.Presentation.Facade
{
    public sealed class ShopFacade
    {
        private readonly ShopView _shopView;

        public ShopFacade(ShopView shopView)
        {
            _shopView = shopView;
        }

        public UniTask RenderAsync(ShopVO[] shops, CancellationToken token)
        {
            _shopView.Render(shops);
            return UniTask.Yield(token);
        }

        public Observable<int> OnClickBuyAsObservable()
        {
            return _shopView.OnClickAnyBuyAsObservable();
        }
    }
}