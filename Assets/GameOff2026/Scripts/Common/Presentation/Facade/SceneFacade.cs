using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FastEnumUtility;
using GameOff2026.Common.Application;
using GameOff2026.Common.Presentation.View;
using GameOff2026.Common.Utility;
using UnityEngine.SceneManagement;

namespace GameOff2026.Common.Presentation.Facade
{
    public sealed class SceneFacade
    {
        private readonly TransitionView _transitionView;

        public SceneFacade(TransitionView transitionView)
        {
            _transitionView = transitionView;
        }

        public UniTask FadeAsync(LoadVO load, CancellationToken token)
        {
            var tween = load.transition.fade switch
            {
                Fade.In => _transitionView.FadeIn(load.transition.duration),
                Fade.Out => _transitionView.FadeOut(load.transition.duration),
                // TODO: Exception
                _ => throw new Exception(),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }

        public async UniTask LoadSceneAsync(SceneName sceneName, CancellationToken token)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName.FastToString());
            // TODO: Exception
            if (asyncOperation == null) throw new Exception();

            asyncOperation.allowSceneActivation = false;
            await UniTask.WaitUntil(() => asyncOperation.progress >= SceneConfig.LOAD_PROGRESS_THRESHOLD,
                PlayerLoopTiming.Update, token);

            GC.Collect();

            asyncOperation.allowSceneActivation = true;
            await asyncOperation
                .ToUniTask(cancellationToken: token);

            for (int i = 0; i < SceneConfig.STABILITY_FRAME; i++)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            await UniTaskHelper.DelayAsync(SceneConfig.STABILITY_TIME, token);
        }
    }
}