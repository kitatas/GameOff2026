using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;

namespace GameOff2026.Game.Presentation.State
{
    public sealed class GameShopState : BaseGameState
    {
        private readonly GameModalUseCase _gameModalUseCase;
        private readonly ShopUseCase _shopUseCase;

        public GameShopState(GameModalUseCase gameModalUseCase, ShopUseCase shopUseCase)
        {
            _gameModalUseCase = gameModalUseCase;
            _shopUseCase = shopUseCase;
        }

        public override GameState state => GameState.Shop;

        public override async UniTask EnterAsync(CancellationToken token)
        {
            await _shopUseCase.PublishShopsAsync(token);
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            var fadeInModal = new GameModalVO(GameModalType.Shop, Fade.In);
            await _gameModalUseCase.FadeAsync(fadeInModal, token);

            var fadeOutModal = new GameModalVO(GameModalType.Shop, Fade.Out);
            await _gameModalUseCase.FadeAsync(fadeOutModal, token);

            return GameState.Return;
        }
    }
}