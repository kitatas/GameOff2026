using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Utility;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Utility
{
    [TestFixture]
    public sealed class AsyncLockLiteTests
    {
        [Test]
        public async Task LockAsync_SecondWaiter_AcquiresAfterFirstRelease()
        {
            using var asyncLock = new AsyncLockLite();
            var first = await asyncLock.LockAsync(CancellationToken.None);
            var secondTask = asyncLock.LockAsync(CancellationToken.None).AsTask();

            Assert.IsFalse(secondTask.IsCompleted);

            first.Dispose();
            var second = await secondTask;
            second.Dispose();

            using var third = await asyncLock.LockAsync(CancellationToken.None);
        }

        [Test]
        public async Task LockAsync_CanceledWaiter_ThrowsOperationCanceledException()
        {
            using var asyncLock = new AsyncLockLite();
            using var source = new CancellationTokenSource();

            var first = await asyncLock.LockAsync(CancellationToken.None);

            try
            {
                var waitingTask = asyncLock.LockAsync(source.Token);

                source.Cancel();

                var exception = default(OperationCanceledException);

                try
                {
                    await waitingTask;
                }
                catch (OperationCanceledException e)
                {
                    exception = e;
                }

                Assert.IsNotNull(exception);
            }
            finally
            {
                first.Dispose();
            }
        }
    }
}
