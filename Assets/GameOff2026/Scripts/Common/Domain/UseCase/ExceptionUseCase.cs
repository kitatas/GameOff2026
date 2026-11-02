using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.Ports;
using MessagePipe;
using R3;

namespace GameOff2026.Common.Domain.UseCase
{
    public sealed class ExceptionUseCase : IDisposable
    {
        private readonly ExceptionPorts _exceptionPorts;
        private readonly Subject<Unit> _decision;

        public ExceptionUseCase(ExceptionPorts exceptionPorts)
        {
            _exceptionPorts = exceptionPorts;
            _decision = new Subject<Unit>();
        }

        public IAsyncSubscriber<ExceptionNotifyVO> exceptionNotify => _exceptionPorts.exceptionNotifySubscriber;
        public IAsyncSubscriber<ExceptionActionVO> exceptionAction => _exceptionPorts.exceptionActionSubscriber;

        public async UniTask ThrowAsync(ExceptionVO ex, CancellationToken token)
        {
            var notify = ExceptionNotifyVO.Create(ex, Fade.In, ExceptionConfig.FADE_DURATION);
            await _exceptionPorts.PublishExceptionNotifyAsync(notify, token);

            await _decision.FirstAsync(token).AsUniTask();

            await FadeOutAsync(ExceptionConfig.FADE_DURATION, token);

            var action = new ExceptionActionVO(ex);
            await _exceptionPorts.PublishExceptionActionAsync(action, token);
        }

        public UniTask ThrowRebootAsync(string message, CancellationToken token)
        {
            return ThrowAsync(new RebootExceptionVO(message), token);
        }

        public UniTask ThrowRetryAsync(string message, CancellationToken token)
        {
            return ThrowAsync(new RetryExceptionVO(message), token);
        }

        public UniTask ThrowQuitAsync(string message, CancellationToken token)
        {
            return ThrowAsync(new QuitExceptionVO(message), token);
        }

        public UniTask FadeOutAsync(float duration, CancellationToken token)
        {
            // NOTE: FadeOut時はException不要なのでnull指定
            var notify = ExceptionNotifyVO.Create(null, Fade.Out, duration);
            return _exceptionPorts.PublishExceptionNotifyAsync(notify, token);
        }

        public void HandleDecision(Unit unit)
        {
            _decision?.OnNext(unit);
        }

        void IDisposable.Dispose()
        {
            _decision?.Dispose();
        }
    }
}