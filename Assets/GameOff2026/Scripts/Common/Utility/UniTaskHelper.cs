using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameOff2026.Common.Utility
{
    public static class UniTaskHelper
    {
        public static CancellationTokenSource BuildLinkedTokenSource(this MonoBehaviour self, CancellationToken token)
        {
            return CancellationTokenSource.CreateLinkedTokenSource(self.GetCancellationTokenOnDestroy(), token);
        }

        public static UniTask DelayAsync(float duration, CancellationToken token)
        {
            return UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: token);
        }
    }
}