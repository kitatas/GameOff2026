using GameOff2026.Common.Application;
using UnityEngine;

namespace GameOff2026.Boot.Application
{
    public sealed class SplashVO
    {
        public readonly SplashType type;
        public readonly Sprite sprite;

        public SplashVO(SplashType type, Sprite sprite)
        {
            if (type is SplashType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_SPLASH);

            this.type = type;
            this.sprite = sprite;
        }
    }

    public sealed class SplashTransitionVO
    {
        public readonly SplashVO splash;
        public readonly TransitionVO transition;

        public SplashTransitionVO(SplashVO splash, TransitionVO transition)
        {
            this.splash = splash;
            this.transition = transition;
        }

        public static SplashTransitionVO Create(SplashVO splash, Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new SplashTransitionVO(splash, transition);
        }
    }
}