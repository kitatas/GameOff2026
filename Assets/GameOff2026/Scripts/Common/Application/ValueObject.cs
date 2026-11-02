using System;
using Cysharp.Text;

namespace GameOff2026.Common.Application
{
    public sealed class TransitionVO
    {
        public readonly Fade fade;
        public readonly float duration;

        public TransitionVO(Fade fade, float duration = 0.0f)
        {
            if (fade is Fade.None) throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE);
            if (duration < 0.0f) throw new QuitExceptionVO(ExceptionConfig.INVALID_DURATION);

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
            if (sceneName is SceneName.None) throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_SCENE);
            if (loadType is LoadType.None) throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_LOAD);

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

    public abstract class ExceptionVO : Exception
    {
        public ExceptionVO(string message) : base(message)
        {
        }

        public virtual string exceptionMessage => ExceptionConfig.QUIT_MESSAGE;
        public string message => ZString.Format("{0}\n{1}", base.Message, exceptionMessage);
    }

    public sealed class RebootExceptionVO : ExceptionVO
    {
        public RebootExceptionVO(string message) : base(message)
        {
        }

        public override string exceptionMessage => ExceptionConfig.REBOOT_MESSAGE;
    }

    public sealed class RetryExceptionVO : ExceptionVO
    {
        public RetryExceptionVO(string message) : base(message)
        {
        }

        public override string exceptionMessage => ExceptionConfig.RETRY_MESSAGE;
    }

    public sealed class QuitExceptionVO : ExceptionVO
    {
        public QuitExceptionVO(string message) : base(message)
        {
        }
    }

    public sealed class ExceptionNotifyVO
    {
        public readonly ExceptionVO exception;
        public readonly TransitionVO transition;

        public ExceptionNotifyVO(ExceptionVO exception, TransitionVO transition)
        {
            this.exception = exception;
            this.transition = transition;
        }

        public static ExceptionNotifyVO Create(ExceptionVO exception, Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new ExceptionNotifyVO(exception, transition);
        }
    }

    public sealed class ExceptionActionVO
    {
        public readonly ExceptionVO exception;

        public ExceptionActionVO(ExceptionVO exception)
        {
            this.exception = exception;
        }
    }
}