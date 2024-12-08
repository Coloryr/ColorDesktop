using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;

namespace ColorDesktop.CoreLib;

public static class ThemeManager
{
    private static readonly Dictionary<string, List<WeakReference<IObserver<IBrush>>>> s_colorList = [];

    private static readonly Dictionary<string, IBrush> s_light = [];
    private static readonly Dictionary<string, IBrush> s_dark = [];

    public static PlatformThemeVariant NowTheme { get; private set; } = PlatformThemeVariant.Light;

    static ThemeManager()
    {
        NowTheme = Application.Current!.PlatformSettings!.GetColorValues().ThemeVariant;
        Application.Current!.PlatformSettings.ColorValuesChanged += PlatformSettings_ColorValuesChanged;

        s_light.Add("FontColor", Brushes.Black);

        s_dark.Add("FontColor", Brushes.White);
    }

    private static void PlatformSettings_ColorValuesChanged(object? sender, PlatformColorValues e)
    {
        NowTheme = e.ThemeVariant;
        Remove();
        Reload();
    }

    public static void AddThemeData(Dictionary<string, IBrush> light, Dictionary<string, IBrush> dark)
    {
        foreach (var item in light)
        {
            lock (s_light)
            {
                s_light.Remove(item.Key);
                s_light.Add(item.Key, item.Value);
            }
        }

        foreach (var item in dark)
        {
            lock (s_dark)
            {
                s_dark.Remove(item.Key);
                s_dark.Add(item.Key, item.Value);
            }
        }

        Reload();
    }

    public static void ClearThemeData()
    {
        s_light.Clear();
        s_dark.Clear();
    }

    public static IBrush GetColor(string key)
    {
        if (NowTheme == PlatformThemeVariant.Light)
        {
            if (s_light.TryGetValue(key, out var color))
            {
                return color;
            }
        }
        else
        {
            if (s_dark.TryGetValue(key, out var color))
            {
                return color;
            }
        }

        return Brushes.Transparent;
    }

    public static void Remove()
    {
        foreach (var item in s_colorList.Values)
        {
            foreach (var item1 in item.ToArray())
            {
                if (!item1.TryGetTarget(out _))
                {
                    item.Remove(item1);
                }
            }
        }
    }

    public static void Reload()
    {
        foreach (var item in s_colorList)
        {
            var value = GetColor(item.Key);
            foreach (var item1 in item.Value)
            {
                if (item1.TryGetTarget(out var target))
                {
                    target.OnNext(value);
                }
            }
        }
    }

    internal static IDisposable Add(string key, IObserver<IBrush> observer)
    {
        if (s_colorList.TryGetValue(key, out var list))
        {
            list.Add(new(observer));
        }
        else
        {
            list = [new(observer)];
            s_colorList.Add(key, list);
        }
        var value = GetColor(key);
        observer.OnNext(value);
        return new UnsubscribeColor(list, observer);
    }
}

public class ThemeExtension(string key) : MarkupExtension, IObservable<IBrush>
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this.ToBinding();
    }

    public IDisposable Subscribe(IObserver<IBrush> observer)
    {
        return ThemeManager.Add(key, observer);
    }
}

internal class UnsubscribeColor(List<WeakReference<IObserver<IBrush>>> observers, IObserver<IBrush> observer) : IDisposable
{
    public void Dispose()
    {
        foreach (var item in observers.ToArray())
        {
            if (!item.TryGetTarget(out var target)
                || target == observer)
            {
                observers.Remove(item);
            }
        }
    }
}