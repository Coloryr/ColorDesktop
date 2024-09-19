using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Platform;
using ColorDesktop.Launcher.Objs;
using ColorDesktop.Launcher.Utils;

namespace ColorDesktop.Launcher.Manager;

public static class ThemeManager
{
    private static readonly Dictionary<string, List<WeakReference<IObserver<IBrush>>>> s_colorList = [];

    private static readonly ThemeObj s_light;
    private static readonly ThemeObj s_dark;

    private static ThemeObj s_theme;

    private static readonly Random s_random = new();

    private static FontFamily s_font = new(FontFamily.DefaultFontFamilyName);

    public static PlatformThemeVariant NowTheme { get; private set; } = PlatformThemeVariant.Light;

    public static void Init()
    {
        if (NowTheme == PlatformThemeVariant.Light)
        {
            s_theme = s_light;
        }
        else
        {
            s_theme = s_dark;
        }

        Reload();

    }

    public static IBrush GetColor(string key)
    {
        if (key == "WindowSideBG")
        {
            return s_theme.WindowSideBG;
        }

        return Brushes.Transparent;
    }

    public static IDisposable Add(string key, IObserver<IBrush> observer)
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

    private static void Reload()
    {
        foreach (var item in s_colorList)
        {
            if (item.Key.StartsWith("Random"))
            {
                foreach (var item1 in item.Value)
                {
                    if (item1.TryGetTarget(out var target))
                    {
                        target.OnNext(GetColor(item.Key));
                    }
                }
            }
            else
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
    }

    static ThemeManager()
    {
        s_light = new()
        {
            WindowSideBG = Brush.Parse("#454545")
        };

        s_dark = new()
        {
            WindowSideBG = Brush.Parse("#454545")
        };
    }
}
