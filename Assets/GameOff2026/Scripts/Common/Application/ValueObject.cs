using System;

namespace GameOff2026.Common.Application
{
    public sealed class TransitionVO
    {
        public readonly Fade fade;
        public readonly float duration;

        public TransitionVO(Fade fade, float duration = 0.0f)
        {
            // TODO: Exception
            if (fade is Fade.None) throw new Exception();
            if (duration < 0.0f) throw new Exception();

            this.fade = fade;
            this.duration = duration;
        }
    }

    public sealed class LoadVO
    {
        public readonly SceneName sceneName;
        public readonly LoadType loadType;
        public readonly TransitionVO transition;

        public LoadVO(SceneName sceneName, LoadType loadType, TransitionVO transition)
        {
            // TODO: Exception
            if (sceneName is SceneName.None) throw new Exception();
            if (loadType is LoadType.None) throw new Exception();

            this.sceneName = sceneName;
            this.loadType = loadType;
            this.transition = transition;
        }

        public static LoadVO CreateFade(SceneName sceneName, Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new LoadVO(sceneName, LoadType.Fade, transition);
        }
    }
}