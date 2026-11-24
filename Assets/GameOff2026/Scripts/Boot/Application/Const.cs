namespace GameOff2026.Boot.Application
{
    public sealed class SplashConfig
    {
        public static readonly SplashType[] TYPES =
        {
            SplashType.Developer,
            SplashType.PlayFab,
        };

        public const float FADE_DURATION = 0.5f;
        public const float DISPLAY_DURATION = 1.0f;
    }
}