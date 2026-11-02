using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace GameOff2026.Common.Presentation.State
{
    public abstract class BaseState<T> where T : Enum
    {
        public abstract T state { get; }

        public virtual UniTask InitAsync(CancellationToken token)
        {
            return UniTask.Yield(token);
        }

        public virtual UniTask EnterAsync(CancellationToken token)
        {
            return UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        public virtual async UniTask<T> TickAsync(CancellationToken token)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            return state;
        }

        public virtual UniTask ExitAsync(CancellationToken token)
        {
            return UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        public virtual UniTask ForceExitAsync(CancellationToken token)
        {
            return UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }
}