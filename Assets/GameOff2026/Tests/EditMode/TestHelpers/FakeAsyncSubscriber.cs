using System;
using MessagePipe;

namespace GameOff2026.Tests.EditMode.TestHelpers
{
    public sealed class FakeAsyncSubscriber<T> : IAsyncSubscriber<T>
    {
        public IDisposable Subscribe(IAsyncMessageHandler<T> asyncHandler,
            params AsyncMessageHandlerFilter<T>[] filters)
        {
            return new EmptyDisposable();
        }

        private sealed class EmptyDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
