using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Domain.UseCase;
using GameOff2026.Common.Presentation.State;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace GameOff2026.Common.Presentation.Presenter
{
    public abstract class BaseStatePresenter<T> : IAsyncStartable, IDisposable where T : Enum
    {
        private readonly BaseStateUseCase<T> _stateUseCase;
        private readonly Dictionary<T, BaseState<T>> _stateMap;
        private readonly CompositeDisposable _disposable;
        private CancellationTokenSource _tokenSource;

        public BaseStatePresenter(BaseStateUseCase<T> stateUseCase, IEnumerable<BaseState<T>> states)
        {
            _stateUseCase = stateUseCase;
            _stateMap = states.ToDictionary(x => x.state, x => x);
            _disposable = new CompositeDisposable();
            _tokenSource = new CancellationTokenSource();
        }

        async UniTask IAsyncStartable.StartAsync(CancellationToken token)
        {
            await UniTask.WhenAll(_stateMap.Values
                .Select(x => x.InitAsync(token)));

            _stateUseCase.subscriber
                .Subscribe(async (s, ct) =>
                {
                    var nextState = await RunAsync(s, ct);
                    await _stateUseCase.PublishAsync(nextState, ct);
                })
                .AddTo(_disposable);

            _stateUseCase.forceChange
                .Subscribe(_ =>
                {
                    _tokenSource?.Cancel();
                    _tokenSource?.Dispose();
                    _tokenSource = new CancellationTokenSource();
                })
                .AddTo(_disposable);

            await _stateUseCase.InitAsync(token);
        }

        private async UniTask<T> RunAsync(T state, CancellationToken token)
        {
            try
            {
                if (!_stateMap.TryGetValue(state, out var currentState))
                {
                    // TODO:
                }

                var nextState = state;

                using var linkedTokenSource =
                    CancellationTokenSource.CreateLinkedTokenSource(token, _tokenSource.Token);

                await currentState.EnterAsync(linkedTokenSource.Token);
                while (EqualityComparer<T>.Default.Equals(nextState, state))
                {
                    linkedTokenSource.Token.ThrowIfCancellationRequested();
                    nextState = await currentState.TickAsync(linkedTokenSource.Token);
                }

                await currentState.ExitAsync(linkedTokenSource.Token);

                return nextState;
            }
            catch (OperationCanceledException)
            {
                if (!_stateMap.TryGetValue(state, out var currentState))
                {
                    // TODO:
                    throw;
                }

                await currentState.ForceExitAsync(token);

                // Stateの強制変更
                return _stateUseCase.forceChangeState;
            }
            catch (Exception)
            {
                // TODO:
                throw;
            }
        }

        public void Dispose()
        {
            _stateUseCase.Dispose();
            _disposable.Dispose();
            _tokenSource.Dispose();
        }
    }
}