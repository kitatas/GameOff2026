namespace GameOff2026.Common.Application
{
    public sealed class SceneConfig
    {
        public const float FADE_DURATION = 0.25f;
        public const float LOAD_PROGRESS_THRESHOLD = 0.9f;
        public const float STABILITY_FRAME = 3;
        public const float STABILITY_TIME = 0.5f;
    }

    public sealed class ExceptionConfig
    {
        public const float FADE_DURATION = 0.5f;
        public const int MAX_RETRY_COUNT = 3;

        public const string REBOOT_MESSAGE = "Return to title.";
        public const string RETRY_MESSAGE = "Retry.";
        public const string QUIT_MESSAGE = "Exit this app.";

        public const string UNKNOWN_ERROR = "UNKNOWN_ERROR";
        public const string MAX_RETRY = "MAX_RETRY";
        public const string FAILED_TO_LOAD_SCENE = "FAILED_TO_LOAD_SCENE";
        public const string INVALID_DURATION = "INVALID_DURATION";
        public const string INVALID_FADE = "INVALID_FADE";
        public const string INVALID_FIELD = "INVALID_FIELD";
        public const string INVALID_WEIGHT = "INVALID_WEIGHT";
        public const string NOT_FOUND_DIRECTION = "NOT_FOUND_DIRECTION";
        public const string NOT_FOUND_LOAD = "NOT_FOUND_LOAD";
        public const string NOT_FOUND_SCENE = "NOT_FOUND_SCENE";
        public const string NOT_FOUND_STATE = "NOT_FOUND_STATE";
    }
}