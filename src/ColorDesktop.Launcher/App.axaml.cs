using System;
using System.IO;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Manager;
using ColorDesktop.Launcher.UI.Models.Dialog;
using ColorDesktop.Launcher.UI.Windows;
using ColorDesktop.Launcher.Utils;
using ColorDesktop.Launcher.ViewModels;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher;

public partial class App : Application
{
    public static LanguageType Lang { get; set; } = LanguageType.zh_cn;

    public static App ThisApp { get; private set; }

    public static MainWindow MainWindow { get; set; }

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
        LangApi.AddLangs(reader.ReadToEnd());
    }

    public static void ShowMainWindow()
    {
        MainWindow ??= new MainWindow();
        MainWindow.WindowState = WindowState.Normal;
        MainWindow.Show();
        MainWindow.Activate();
    }

    public static async void Exit()
    {
        ShowMainWindow();
        var res = await DialogHost.Show(new ChoiseModel()
        {
            Text = "是否要退出软件"
        }, MainWindow.DialogHostName);
        if (res is true)
        {
            InstanceManager.StopInstance();
            PluginManager.StopPlugin();

            (ThisApp.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Shutdown();
            ConfigSave.Stop();
            Logs.Stop();

            Environment.Exit(Environment.ExitCode);
        }
    }

    public App()
    {
        ThisApp = this;

        Program.StartLock();
        LauncherHook.Init(new InstanceHook());

        Logs.Init(Program.RunDir, Program.Version);
    }

    public void UpdateMenu()
    {
        if (DataContext is AppModel model)
        {
            model.Update();
        }
    }

    public override void Initialize()
    {
        LoadLanguage(Lang);

        ConfigHelper.LoadConfig();

        ThemeManager.Init();
        ConfigSave.Init();

        PluginManager.Init();
        InstanceManager.Init();
        TempManager.Init();

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

        if (PlatformSettings is { } setting)
        {
            setting.ColorValuesChanged += (object? sender, PlatformColorValues e) =>
            {
                ThemeManager.Init();
            };
        }

        ShowMainWindow();

        base.OnFrameworkInitializationCompleted();
    }

    private void Life_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        ConfigSave.Stop();
        LaunchSocketUtils.Stop();
    }
}
