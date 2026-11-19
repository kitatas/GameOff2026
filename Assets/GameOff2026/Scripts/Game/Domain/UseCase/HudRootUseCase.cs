using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.Ports;
using MessagePipe;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class HudRootUseCase
    {
        private readonly HudRootPorts _hudRootPorts;

        public HudRootUseCase(HudRootPorts hudRootPorts)
        {
            _hudRootPorts = hudRootPorts;
        }

        public IAsyncSubscriber<HudRootTransitionVO> transition => _hudRootPorts.hudRootTransitionSubscriber;

        public UniTask InitAsync(CancellationToken token)
        {
            return FadeAsync(Fade.Out, 0.0f, token);
        }

        public UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            return FadeAsync(fade, HudRootConfig.FADE_DURATION, token);
        }

        private UniTask FadeAsync(Fade fade, float duration, CancellationToken token)
        {
            var hudRootTransition = HudRootTransitionVO.Create(fade, duration);
            return _hudRootPorts.PublishHudRootTransitionAsync(hudRootTransition, token);
        }
    }
}