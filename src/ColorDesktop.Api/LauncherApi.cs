using System.Collections.Concurrent;

namespace ColorDesktop.Api;

public static class LauncherApi
{
    /// <summary>
    /// API版本
    /// </summary>
    public const string ApiVersion = "3";

    public static ILauncherHook Hook { get; private set; }

    private static readonly ConcurrentDictionary<string, object?> s_shareData = [];

    public static bool GetData(string key, out object? obj)
    {
        return s_shareData.TryGetValue(key, out obj);
    }

    public static void SetData(string key, object? obj)
    {
        if (!s_shareData.TryAdd(key, obj))
        {
            s_shareData[key] = obj;
        }
    }

    public static void Init(ILauncherHook hook)
    {
        Hook ??= hook;
    }
}
