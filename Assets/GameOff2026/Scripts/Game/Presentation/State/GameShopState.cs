using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameShopState : BaseGameState
    {
        private readonly ShopUseCase _shopUseCase;

        public GameShopState(ShopUseCase shopUseCase)
        {
            _shopUseCase = shopUseCase;
        }

        public override GameState state => GameState.Shop;

        public override async UniTask EnterAsync(CancellationToken token)
        {
            await _shopUseCase.PublishShopsAsync(token);
        }

        public override UniTask<GameState> TickAsync(CancellationToken token)
        {
            return UniTask.FromResult(GameState.SetUp);
        }
    }
}