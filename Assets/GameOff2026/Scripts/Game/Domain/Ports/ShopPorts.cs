using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using MessagePipe;

namespace GameOff2026.Game.Domain.Ports
{
    public sealed class ShopPorts
    {
        public readonly IAsyncSubscriber<ShopVO[]> shopSubscriber;
        private readonly IAsyncPublisher<ShopVO[]> _shopPublisher;

        public ShopPorts(IAsyncSubscriber<ShopVO[]> shopSubscriber, IAsyncPublisher<ShopVO[]> shopPublisher)
        {
            this.shopSubscriber = shopSubscriber;
            _shopPublisher = shopPublisher;
        }

        public UniTask PublishShopsAsync(ShopVO[] shops, CancellationToken token)
        {
            return _shopPublisher.PublishAsync(shops, token);
        }
    }
}