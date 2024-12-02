namespace ColorDesktop.Api;

public static class LauncherApi
{
    /// <summary>
    /// API版本
    /// </summary>
    public const string ApiVersion = "3";

    private static ILauncherHook s_hook;

    public static void Init(ILauncherHook hook)
    {
        s_hook ??= hook;
    }
}
