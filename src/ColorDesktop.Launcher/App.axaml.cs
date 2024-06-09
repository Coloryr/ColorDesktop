using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Threading;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Objs;
using ColorDesktop.Launcher.Utils;
using ColorDesktop.Launcher.ViewModels;
using ColorDesktop.Launcher.ViewModels.Dialog;
using ColorDesktop.Launcher.Views;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher;

public partial class App : Application
{
    private static readonly Language s_language = new();

    public static MainWindow? MainWindow { get; set; }

    private static List<(Window, InstanceDataObj)> Windows = [];

    private static Application ThisApp;

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

    public static (bool, string?) EnablePlugin(string id)
    {
        ConfigHelper.EnablePlugin(id);

        return (true, null);
    }

    public static (bool, string?) DisablePlugin(string id)
    {
        ConfigHelper.DisablePlugin(id);
        return (true, null);
    }

    public static string MakeUUID()
    {
        string uuid;
        do
        {
            uuid = Guid.NewGuid().ToString().Replace("-", "").ToLower();
        }
        while (InstanceMan.KnowUUID.Contains(uuid));

        return uuid;
    }

    /// <summary>
    /// 新建一个显示实例
    /// </summary>
    /// <param name="obj"></param>
    public static async void CreateInstance(PluginDataObj obj)
    {
        if (PluginMan.PluginAssemblys.TryGetValue(obj.ID, out var value))
        {
            var config = value.Plugin.CreateInstanceDefault();
            config.UUID = MakeUUID();
            var config1 = await DialogHost.Show(new CreateInstanceModel(config) ,"MainWindow");
            if (config1 is not InstanceDataObj obj1)
            {
                return;
            }
            var control = value.Plugin.CreateInstanceSetting(obj1);
            if (control != null)
            {
                await control.ShowDialog(MainWindow!);
            }

            InstanceMan.Create(obj1);
            MainWindow?.LoadInstance();
            InitInstance(obj1);
        }
    }

    public static void OpenSetting(PluginDataObj obj)
    {
        if (PluginMan.PluginAssemblys.TryGetValue(obj.ID, out var value))
        {
            value.Plugin.OpenSetting();
        }
    }

    /// <summary>
    /// 开启一个显示实例
    /// </summary>
    /// <param name="obj"></param>
    public static void InitInstance(InstanceDataObj obj)
    {
        if (PluginMan.PluginAssemblys.TryGetValue(obj.Plugin, out var value))
        {
            var view = value.Plugin.MakeInstances(InstanceMan.GetLocal(obj), obj);
            Window window;
            if (obj.IsWindow)
            {
                window = (view.CreateView() as Window)!;
            }
            else
            {
                var window1 = new InstanceWindow();
                window1.Load(view, obj);
                window = window1;
            }

            Windows.Add((window, obj));
            window.Show();

            view.Start();

            Dispatcher.UIThread.Post(() =>
            {
                Move(window, obj);
                window.PositionChanged += (a, b) =>
                {
                    // 获取当前屏幕
                    var screen = window.Screens.Primary;
                    if (screen == null)
                    {
                        return;
                    }
                    var workArea = screen.WorkingArea;

                    for (int i = 0; i < window.Screens.All.Count; i++)
                    {
                        if (window.Screens.All[i] == screen)
                        {
                            obj.Display = i + 1;
                            break;
                        }
                    }
                    // 计算新的 Margin
                    obj.Margin.Left = window.Position.X - workArea.X;
                    obj.Margin.Top = window.Position.Y - workArea.Y;
                    obj.Margin.Right = workArea.X + workArea.Width - (window.Position.X + (int)window.Width);
                    obj.Margin.Bottom = workArea.Y + workArea.Height - (window.Position.Y + (int)window.Height);

                    ConfigSave.AddItem(new ConfigSaveObj()
                    {
                        Name = obj.UUID + "json",
                        Local = InstanceMan.GetDataLocal(obj),
                        Obj = obj
                    });
                };
            });
        }
    }

    public static void Move(Window window, InstanceDataObj obj)
    {
        // 获取所有显示器的信息
        var screens = window.Screens.All;

        Screen? targetScreen;
        if (obj.Display != 1 && screens.Count > obj.Display - 1)
        {
            targetScreen = screens[obj.Display - 1];
        }
        else
        {
            targetScreen = window.Screens.Primary;
        }

        if (targetScreen != null)
        {
            var margin = obj.Margin;
            var workArea = targetScreen.WorkingArea;
            int x = 0;
            int y = 0;

            switch (obj.Pos)
            {
                case PosEnum.TopLeft:
                    x = workArea.X + margin.Left;
                    y = workArea.Y + margin.Top;
                    break;
                case PosEnum.Top:
                    x = workArea.X + (workArea.Width - (int)window.Width) / 2 + margin.Left - margin.Right;
                    y = workArea.Y + margin.Top;
                    break;
                case PosEnum.TopRight:
                    x = workArea.X + workArea.Width - (int)window.Width - margin.Right;
                    y = workArea.Y + margin.Top;
                    break;
                case PosEnum.Left:
                    x = workArea.X + margin.Left;
                    y = workArea.Y + (workArea.Height - (int)window.Height) / 2 + margin.Top - margin.Bottom;
                    break;
                case PosEnum.Center:
                    x = workArea.X + (workArea.Width - (int)window.Width) / 2 + margin.Left - margin.Right;
                    y = workArea.Y + (workArea.Height - (int)window.Height) / 2 + margin.Top - margin.Bottom;
                    break;
                case PosEnum.Right:
                    x = workArea.X + workArea.Width - (int)window.Width - margin.Right;
                    y = workArea.Y + (workArea.Height - (int)window.Height) / 2 + margin.Top - margin.Bottom;
                    break;
                case PosEnum.BottomLeft:
                    x = workArea.X + margin.Left;
                    y = workArea.Y + workArea.Height - (int)window.Height - margin.Bottom;
                    break;
                case PosEnum.Bottom:
                    x = workArea.X + (workArea.Width - (int)window.Width) / 2 + margin.Left - margin.Right;
                    y = workArea.Y + workArea.Height - (int)window.Height - margin.Bottom;
                    break;
                case PosEnum.BottomRight:
                    x = workArea.X + workArea.Width - (int)window.Width - margin.Right;
                    y = workArea.Y + workArea.Height - (int)window.Height - margin.Bottom;
                    break;
            }

            window.Position = new PixelPoint(x, y);
        }
    }

    public static void StartPlugin()
    {
        foreach (var item in ConfigHelper.Config.EnablePlugin)
        {
            if (PluginMan.PluginAssemblys.TryGetValue(item, out var ass))
            {
                ass.Plugin.Init(ass.Local, LanguageType.zh_cn);
            }
        }

        foreach (var item in ConfigHelper.Config.EnableInstance)
        {
            if (InstanceMan.Instances.TryGetValue(item, out var obj))
            {
                InitInstance(obj);
            }
        }
    }

    public override void Initialize()
    {
        ThisApp = this;

        SystemInfo.Init();
        ConfigSave.Init();

        LoadLanguage(LanguageType.zh_cn);

        ConfigHelper.LoadConfig();

        PluginMan.Init();
        InstanceMan.Init();

        foreach (var item1 in PluginMan.LoadFail)
        {
            ConfigHelper.Config.EnablePlugin.Remove(item1.Item1);
        }
        foreach (var item1 in InstanceMan.LoadFail)
        {
            ConfigHelper.Config.EnableInstance.Remove(item1.Key);
        }

        ConfigHelper.SaveConfig();

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
