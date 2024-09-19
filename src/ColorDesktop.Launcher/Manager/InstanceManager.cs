using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Threading;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Objs;
using ColorDesktop.Launcher.UI.Models.Dialog;
using ColorDesktop.Launcher.UI.Windows;
using ColorDesktop.Launcher.Utils;
using DialogHostAvalonia;
using Newtonsoft.Json;

namespace ColorDesktop.Launcher.Manager;

public static class InstanceManager
{
    private static List<InstanceWindowObj> Windows = [];

    public static readonly List<string> KnowUUID = [];
    public static readonly Dictionary<string, InstanceDataObj> Instances = [];

    /// <summary>
    /// 读取时失败
    /// </summary>
    public static readonly Dictionary<string, Exception> LoadError = [];
    /// <summary>
    /// 加载时失败
    /// </summary>
    public static readonly Dictionary<string, Exception> LoadFail = [];

    public const string Dir2 = "instances";
    public const string FileName = "instance.json";

    private static string s_workDir;

    public static void Init()
    {
        s_workDir = Path.GetFullPath(AppContext.BaseDirectory + Dir2);
        Directory.CreateDirectory(s_workDir);
        var info = new DirectoryInfo(s_workDir);
        foreach (var item in info.GetDirectories())
        {
            try
            {
                var uuid = item.Name;
                KnowUUID.Add(uuid);
                var config = item.GetFiles().FirstOrDefault(item => item.Name.Equals(FileName, StringComparison.CurrentCultureIgnoreCase));
                if (config != null)
                {
                    var obj = JsonConvert.DeserializeObject<InstanceDataObj>(File.ReadAllText(config.FullName));
                    if (obj == null)
                    {
                        continue;
                    }
                    if (obj.UUID != uuid)
                    {
                        obj.UUID = uuid;
                        ConfigUtils.Save(obj, config.FullName);
                    }
                    Instances.Add(uuid, obj);
                }
            }
            catch (Exception e)
            {
                LoadError.Add(item.Name, e);
            }
        }

        foreach (var item in Instances)
        {
            if (!PluginManager.PluginAssemblys.ContainsKey(item.Value.Plugin))
            {
                LoadFail.Add(item.Key, new Exception("not found plugin " + item.Value.Plugin));
            }
        }

        foreach (var item1 in LoadFail)
        {
            ConfigHelper.Config.EnableInstance.Remove(item1.Key);
        }
    }

    /// <summary>
    /// 新建一个显示实例
    /// </summary>
    /// <param name="obj"></param>
    public static async void CreateInstance(PluginDataObj obj)
    {
        if (PluginManager.PluginAssemblys.TryGetValue(obj.ID, out var value))
        {
            var config = value.Plugin.CreateInstanceDefault();
            config.UUID = MakeUUID();
            var config1 = await DialogHost.Show(new CreateInstanceModel(config), "MainWindow");
            if (config1 is not InstanceDataObj obj1)
            {
                return;
            }
            var control = value.Plugin.CreateInstanceSetting(obj1);
            if (control != null)
            {
                await control.ShowDialog(App.MainWindow!);
            }

            Create(obj1);
            App.MainWindow?.LoadInstance();
            InitInstance(obj1);
        }
    }

    /// <summary>
    /// 开启一个显示实例
    /// </summary>
    /// <param name="obj"></param>
    public static void InitInstance(InstanceDataObj obj)
    {
        if (PluginManager.PluginAssemblys.TryGetValue(obj.Plugin, out var value))
        {
            var view = value.Plugin.MakeInstances(GetLocal(obj), obj);
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

            Windows.Add(new(window, obj));
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
                        Local = GetDataLocal(obj),
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

    private static string MakeUUID()
    {
        string uuid;
        do
        {
            uuid = Guid.NewGuid().ToString().Replace("-", "").ToLower();
        }
        while (KnowUUID.Contains(uuid));

        return uuid;
    }

    public static void Create(InstanceDataObj obj)
    {
        var dir = GetLocal(obj);
        Directory.CreateDirectory(dir);
        ConfigUtils.Save(obj, Path.GetFullPath(dir + "/" + FileName));
        Instances.Add(obj.UUID, obj);
        KnowUUID.Add(obj.UUID);
        ConfigHelper.EnableInstance(obj.UUID);
    }

    public static string GetLocal(InstanceDataObj obj)
    {
        return Path.GetFullPath(s_workDir + "/" + obj.UUID);
    }

    public static string GetDataLocal(InstanceDataObj obj)
    {
        return Path.GetFullPath(s_workDir + "/" + obj.UUID + "/" + FileName);
    }

    public static void StartInstance()
    {
        foreach (var item in ConfigHelper.Config.EnableInstance)
        {
            if (Instances.TryGetValue(item, out var obj))
            {
                InitInstance(obj);
            }
        }
    }
}
