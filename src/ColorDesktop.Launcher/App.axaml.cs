using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Objs;
using ColorDesktop.Launcher.Utils;
using ColorDesktop.Launcher.ViewModels;
using ColorDesktop.Launcher.Views;

namespace ColorDesktop.Launcher;

public partial class App : Application
{
    private static readonly Language s_language = new();

    public static ConfigObj Config;

    public static MainWindow? MainWindow { get; set; }

    private static List<(Window, InstanceDataObj)> Windows = [];

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
        if (Config.EnablePlugin.Contains(id))
        {
            return (true, null);
        }

        Config.EnablePlugin.Add(id);
        SaveConfig();
        return (true, null);
    }

    public static (bool, string?) DisablePlugin(string id)
    {
        if (!Config.EnablePlugin.Contains(id))
        {
            return (true, null);
        }

        Config.EnablePlugin.Remove(id);
        SaveConfig();
        return (true, null);
    }

    public static void LoadConfig()
    {
        Config = ConfigUtils.Config(new ConfigObj()
        {
            EnablePlugin = [],
            EnableInstance = []
        }, AppContext.BaseDirectory + "config.json");

        Config.EnablePlugin ??= [];
        Config.EnableInstance ??= [];
    }

    public static void SaveConfig()
    {
        ConfigUtils.Save(Config, "config.json");
    }

    public static string MakeUUID()
    {
        string uuid;
        do
        {
            uuid = Guid.NewGuid().ToString().Replace("-", "").ToLower();
        }
        while (InstancnMan.KnowUUID.Contains(uuid));

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
            var config = await value.Plugin.CreateInstances();
            config.UUID = MakeUUID();
            var local = InstancnMan.Create(config);
            var view = value.Plugin.MakeInstances(local, config);
            Window window;
            if (config.IsWindow)
            {
                window = (view.CreateView() as Window)!;
            }
            else
            {
                var window1 = new InstanceWindow();
                window1.Load(view, config);
                window = window1;
            }

            Windows.Add((window, config));
            window.Show();

            view.Start();
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
    public static async void InitInstance(InstanceDataObj obj)
    {
        if (PluginMan.PluginAssemblys.TryGetValue(obj.Plugin, out var value))
        {
            
        }
    }

    public override void Initialize()
    {
        SystemInfo.Init();

        LoadLanguage(LanguageType.zh_cn);

        LoadConfig();

        PluginMan.Init();
        InstancnMan.Init();

        foreach (var item in Config.EnablePlugin.ToArray())
        {
            foreach (var item1 in PluginMan.LoadFail)
            {
                if (item1.Item1 == item)
                {
                    Config.EnablePlugin.Remove(item);
                    break;
                }
            }
        }
        foreach (var item in Config.EnableInstance.ToArray())
        {
            foreach (var item1 in InstancnMan.LoadFail)
            {
                if (item1.Key == item)
                {
                    Config.EnableInstance.Remove(item);
                    break;
                }
            }
        }

        foreach (var item in Config.EnablePlugin)
        {
            if (PluginMan.PluginAssemblys.TryGetValue(item, out var ass))
            {
                ass.Plugin.Init(ass.Local, LanguageType.zh_cn);
            }
        }

        foreach (var item in Config.EnableInstance)
        {
            if (InstancnMan.Instancns.TryGetValue(item, out var obj))
            {
                //InitInstance(obj);
            }
        }

        AvaloniaXamlLoader.Load(this);

        DataContext = new AppModel();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        ShowMain();

        base.OnFrameworkInitializationCompleted();
    }
}
