using System;
using System.IO;
using System.Reflection;
using Avalonia;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

namespace ColorDesktop.Debug;

public partial class App : Application
{

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        base.OnFrameworkInitializationCompleted();
    }
}
