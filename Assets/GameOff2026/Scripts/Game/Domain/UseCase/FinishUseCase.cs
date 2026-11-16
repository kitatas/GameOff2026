using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.Ports;
using MessagePipe;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class FinishUseCase
    {
        private readonly FinishPorts _finishPorts;

        public FinishUseCase(FinishPorts finishPorts)
        {
            _finishPorts = finishPorts;
        }

        public IAsyncSubscriber<FinishTransitionVO> finishTransition => _finishPorts.finishTransitionSubscriber;

        public UniTask InitAsync(FinishType type, CancellationToken token)
        {
            var f = FinishTransitionVO.Create(type, Fade.Out, 0.0f);
            return _finishPorts.PublishFinishAsync(f, token);
        }

        public UniTask FadeAsync(FinishType type, Fade fade, CancellationToken token)
        {
            var f = FinishTransitionVO.Create(type, fade, FinishConfig.FADE_DURATION);
            return _finishPorts.PublishFinishAsync(f, token);
        }
    }
}