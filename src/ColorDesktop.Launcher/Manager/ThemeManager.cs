﻿using System;
using System.Collections.Generic;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using ColorDesktop.Launcher.Objs;
using ColorDesktop.Launcher.Utils;

namespace ColorDesktop.Launcher.Manager;

public static class ThemeManager
{
    public const string MainColorStr = "#FF5ABED6";

    private static readonly Dictionary<string, List<WeakReference<IObserver<IBrush>>>> s_colorList = [];
    private static readonly Dictionary<string, List<WeakReference<IObserver<object?>>>> s_styleList = [];

    private static readonly ThemeObj s_light;
    private static readonly ThemeObj s_dark;

    private static ThemeObj s_theme;

    private static readonly Random s_random = new();

    private static FontFamily s_font = new(FontFamily.DefaultFontFamilyName);

    private static BoxShadows s_buttonShadow;

    public static readonly BoxShadows BorderShadows = new(BoxShadow.Parse("0 0 3 1 #1A000000"), [BoxShadow.Parse("0 0 5 -1 #1A000000")]);
    public static BoxShadows BorderSelecrShadows { get; private set; }

    public static PlatformThemeVariant NowTheme { get; private set; } = PlatformThemeVariant.Light;

    public static void Init()
    {
        NowTheme = App.ThisApp.PlatformSettings!.GetColorValues().ThemeVariant;
        if (NowTheme == PlatformThemeVariant.Light)
        {
            s_theme = s_light;
        }
        else
        {
            s_theme = s_dark;
        }

        Reload();
        LoadColor();
    }

    public static Color ToColor(this IBrush brush)
    {
        if (brush is ImmutableSolidColorBrush brush1)
        {
            return brush1.Color;
        }

        return new(255, 255, 255, 255);
    }

    private static void LoadColor()
    {
        s_theme.MainColor = Brush.Parse(MainColorStr);
        var color = s_theme.MainColor.ToColor();
        var color1 = new Color(255, color.R, color.G, color.B);

        s_buttonShadow = new(new BoxShadow
        {
            Blur = 3,
            Spread = 1,
            Color = color1
        });

        BorderSelecrShadows = new(new BoxShadow
        {
            Blur = 3,
            Spread = 1,
            Color = color1
        });
    }

    public static IBrush GetColor(string key)
    {
        if (key == nameof(ThemeObj.WindowBG))
        {
            return s_theme.WindowBG;
        }
        else if (key == nameof(ThemeObj.WindowSideBG))
        {
            return s_theme.WindowSideBG;
        }
        else if (key == nameof(ThemeObj.WindowSideFont))
        {
            return s_theme.WindowSideFont;
        }
        else if (key == nameof(ThemeObj.WindowSideBGTop))
        {
            return s_theme.WindowSideBGTop;
        }
        else if (key == nameof(ThemeObj.WindowSideBGSelect))
        {
            return s_theme.WindowSideBGSelect;
        }
        else if (key == nameof(ThemeObj.ControlBorder))
        {
            return s_theme.ControlBorder;
        }
        else if (key == nameof(ThemeObj.ControlTopBGColor))
        {
            return s_theme.ControlTopBGColor;
        }
        else if (key == nameof(ThemeObj.ButtonOver))
        {
            return s_theme.ButtonOver;
        }
        else if (key == nameof(ThemeObj.ButtonBorder))
        {
            return s_theme.ButtonBorder;
        }
        else if (key == nameof(ThemeObj.ButtonBG))
        {
            return s_theme.ButtonBG;
        }
        else if (key == nameof(ThemeObj.FontColor))
        {
            return s_theme.FontColor;
        }
        else if (key == nameof(ThemeObj.MainColor))
        {
            return s_theme.MainColor;
        }
        else if (key == nameof(ThemeObj.ViewBG))
        {
            return s_theme.ViewBG;
        }
        else if (key == nameof(ThemeObj.ViewBorder))
        {
            return s_theme.ViewBorder;
        }
        else if (key == nameof(ThemeObj.ItemOverBG))
        {
            return s_theme.ItemOverBG;
        }

        return Brushes.Transparent;
    }

    private static object? GetStyle(string key)
    {
        if (key == "ButtonTopBoxShadow")
        {
            return s_buttonShadow;
        }
        return null;
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

    public static IDisposable AddStyle(string key, IObserver<object?> observer)
    {
        if (s_styleList.TryGetValue(key, out var list))
        {
            list.Add(new(observer));
        }
        else
        {
            list = [new(observer)];
            s_styleList.Add(key, list);
        }
        var value = GetStyle(key);
        observer.OnNext(value);
        return new UnsubscribeStyle(list, observer);
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
            WindowBG = Brush.Parse("#99FFFFFF"),
            WindowSideBG = Brush.Parse("#FF454545"),
            WindowSideFont = Brush.Parse("#FFFFFFFF"),
            WindowSideBGTop = Brush.Parse("#FF565656"),
            WindowSideBGSelect = Brush.Parse("#FF787878"),
            ControlBorder = Brush.Parse("#FFe5e7eb"),
            ControlTopBGColor = Brush.Parse("#EFFFFFFF"),
            ButtonBG = Brush.Parse("#FFFFFFFF"),
            ButtonOver = Brush.Parse("#FFFEFEFE"),
            ButtonBorder = Brush.Parse("#FFD4D4D8"),
            FontColor = Brush.Parse("#FF000000"),
            ViewBG = Brush.Parse("#DDFFFFFF"),
            ViewBorder = Brush.Parse("#FFe5e5e5"),
            ItemOverBG = Brush.Parse("#DDEFEFEF"),
        };

        s_dark = new()
        {
            WindowBG = Brush.Parse("#99000000"),
            WindowSideBG = Brush.Parse("#FF454545"),
            WindowSideFont = Brush.Parse("#FFFFFFFF"),
            WindowSideBGTop = Brush.Parse("#FF565656"),
            WindowSideBGSelect = Brush.Parse("#787878"),
            ControlBorder = Brush.Parse("#FFe5e7eb"),
            ControlTopBGColor = Brush.Parse("#EF000000"),
            ButtonBG = Brush.Parse("#FF000000"),
            ButtonOver = Brush.Parse("#FF141414"),
            ButtonBorder = Brush.Parse("#FFD4D4D8"),
            FontColor = Brush.Parse("#FFFFFFFF"),
            ViewBG = Brush.Parse("#CF27272a"),
            ViewBorder = Brush.Parse("#FF1d1d1d"),
            ItemOverBG = Brush.Parse("#DD27272a"),
        };
    }
}
