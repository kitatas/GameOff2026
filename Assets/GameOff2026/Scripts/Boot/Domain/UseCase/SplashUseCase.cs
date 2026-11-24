using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Boot.Application;
using GameOff2026.Boot.Domain.Ports;
using GameOff2026.Boot.Domain.Repository;
using GameOff2026.Common.Application;
using MessagePipe;
using R3;

namespace GameOff2026.Boot.Domain.UseCase
{
    public sealed class SplashUseCase : IDisposable
    {
        private readonly SplashPorts _splashPorts;
        private readonly SplashRepository _splashRepository;
        private readonly Subject<Unit> _tapScreen;

        public SplashUseCase(SplashPorts splashPorts, SplashRepository splashRepository)
        {
            _splashPorts = splashPorts;
            _splashRepository = splashRepository;
            _tapScreen = new Subject<Unit>();
        }

        public IAsyncSubscriber<SplashTransitionVO> transition => _splashPorts.splashTransitionSubscriber;

        public UniTask InitAsync(CancellationToken token)
        {
            // NOTE: FadeOut時はSplash不要なのでnull指定
            var splashTransition = SplashTransitionVO.Create(null, Fade.Out, 0.0f);
            return _splashPorts.PublishSplashTransitionAsync(splashTransition, token);
        }

        public async UniTask SequentialRenderAsync(CancellationToken token)
        {
            foreach (var type in SplashConfig.TYPES)
            {
                using var iterationCts = CancellationTokenSource.CreateLinkedTokenSource(token);

                var result = await UniTask.WhenAny(
                    FadeAsync(type, iterationCts.Token),
                    _tapScreen.FirstAsync(cancellationToken: token).AsUniTask()
                );

                // tapScreen であれば Tween cancel
                if (result == 1) iterationCts.Cancel();

                await UniTask.Yield(token);
            }
        }

        private UniTask FadeAsync(SplashType type, CancellationToken token)
        {
            var splash = _splashRepository.Find(type);
            var splashTransition = SplashTransitionVO.Create(splash, Fade.InOut, SplashConfig.FADE_DURATION);
            return _splashPorts.PublishSplashTransitionAsync(splashTransition, token);
        }

        public void NotifyTapScreen()
        {
            _tapScreen?.OnNext(Unit.Default);
        }

        void IDisposable.Dispose()
        {
            _tapScreen?.Dispose();
        }
    }
}