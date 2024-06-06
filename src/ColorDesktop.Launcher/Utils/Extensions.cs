using System;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

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
