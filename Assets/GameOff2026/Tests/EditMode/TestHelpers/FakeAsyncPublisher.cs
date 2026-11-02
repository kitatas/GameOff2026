using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace GameOff2026.Tests.EditMode.TestHelpers
{
    public sealed class FakeAsyncPublisher<T> : IAsyncPublisher<T>
    {
        public readonly List<T> published = new();
        public readonly List<CancellationToken> tokens = new();

        public int PublishCount => published.Count;
        public T LastPublished => published[published.Count - 1];

        public void Publish(T message, CancellationToken cancellationToken = default)
        {
            Record(message, cancellationToken);
        }

        public UniTask PublishAsync(T message, CancellationToken cancellationToken = default)
        {
            Record(message, cancellationToken);
            return UniTask.CompletedTask;
        }

        public UniTask PublishAsync(T message, AsyncPublishStrategy publishStrategy,
            CancellationToken cancellationToken = default)
        {
            Record(message, cancellationToken);
            return UniTask.CompletedTask;
        }

        private void Record(T message, CancellationToken token)
        {
            published.Add(message);
            tokens.Add(token);
        }
    }
}
