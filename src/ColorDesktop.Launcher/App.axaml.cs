using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Threading;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Manager;
using ColorDesktop.Launcher.UI.Windows;
using ColorDesktop.Launcher.Utils;
using ColorDesktop.Launcher.ViewModels;

namespace ColorDesktop.Launcher;

public partial class App : Application
{
    private static readonly Language s_language = new();

    private static Application ThisApp;

    public static MainWindow? MainWindow { get; set; }

    public static string Lang(string key)
    {
        return s_language.GetLanguage(key);
    }

    public static void LoadLanguage(LanguageType type)
    {
        var assm = Assembly.GetExecutingAssembly();
        if (assm == null)
        {
            return;
        }
        string name = type switch
        {
            LanguageType.en_us => "ColorDesktop.Launcher.Resource.Lang.en-us.json",
            _ => "ColorDesktop.Launcher.Resource.Lang.zh-cn.json"
        };
        using var item = assm.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(item);
        s_language.Load(reader.ReadToEnd());
    }

    public static void ShowMain()
    {
        MainWindow ??= new MainWindow();
        MainWindow.Show();
        MainWindow.Activate();
    }

    public static void Exit()
    { 
        
    }

    public override void Initialize()
    {
        ThisApp = this;

        SystemInfo.Init();
        ConfigSave.Init();

        LoadLanguage(LanguageType.zh_cn);

        ConfigHelper.LoadConfig();

        ThemeManager.Init();

        PluginManager.Init();
        InstanceManager.Init();

        AvaloniaXamlLoader.Load(this);

        DataContext = new AppModel();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is ClassicDesktopStyleApplicationLifetime life)
        {
            life.Exit += Life_Exit;
        }

        ShowMain();

        base.OnFrameworkInitializationCompleted();
    }

    private void Life_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        ConfigSave.Stop();
    }
}
