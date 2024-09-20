using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ColorDesktop.Launcher.Manager;

namespace ColorDesktop.Launcher.Utils;

public class LocalizeExtension(string key) : MarkupExtension, IObservable<string>
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this.ToBinding();
    }

    public IDisposable Subscribe(IObserver<string> observer)
    {
        return LangSel.Add(key, observer);
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

public class StyleExtension(string key) : MarkupExtension, IObservable<object?>
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this.ToBinding();
    }

    public IDisposable Subscribe(IObserver<object?> observer)
    {
        return ThemeManager.AddStyle(key, observer);
    }
}

public class UnsubscribeColor(List<WeakReference<IObserver<IBrush>>> observers, IObserver<IBrush> observer) : IDisposable
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

public class UnsubscribeStyle(List<WeakReference<IObserver<object?>>> observers, IObserver<object?> observer) : IDisposable
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

public class UnsubscribeLang(List<WeakReference<IObserver<string>>> observers, IObserver<string> observer) : IDisposable
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
