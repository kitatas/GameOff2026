using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.Ports;
using MessagePipe;
using R3;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class TapScreenUseCase : IDisposable
    {
        private readonly TapScreenPorts _tapScreenPorts;
        private readonly Subject<Unit> _tapScreen;

        public TapScreenUseCase(TapScreenPorts tapScreenPorts)
        {
            _tapScreenPorts = tapScreenPorts;
            _tapScreen = new Subject<Unit>();
        }

        public IAsyncSubscriber<TapScreenTransitionVO> tapScreenTransition => _tapScreenPorts.tapScreenTransitionSubscriber;

        public UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            var tap = TapScreenTransitionVO.Create(fade, TapScreenConfig.FADE_DURATION);
            return _tapScreenPorts.PublishTapScreenAsync(tap, token);
        }

        public void NotifyTapScreen()
        {
            _tapScreen?.OnNext(Unit.Default);
        }

        public UniTask WaitForTapScreenAsync(CancellationToken token)
        {
            return _tapScreen
                .FirstAsync(cancellationToken: token)
                .AsUniTask();
        }

        void IDisposable.Dispose()
        {
            _tapScreen?.Dispose();
        }
    }
}