using System.Reflection;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;
using HidSharp.Platform;
using LibreHardwareMonitor.Hardware;

namespace ColorDesktop.MonitorPlugin;

public class MonitorPlugin : IPlugin
{
    public const string ConfigName = "monitor.json";

    public static MonitorConfigObj Config { get; set; }

    public static Computer Computer { get; private set; }
    public static UpdateVisitor UpdateVisitor = new();

    private static string s_local;

    private static Thread _timer;
    private static bool _run;
    private static bool _gethw;

    public static MonitorInstanceObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new MonitorInstanceObj()
        {
            Items = [],
            AutoSize = false,
            Width = 300,
            Height = 500,
            PanelType = PanelType.Wrap
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, MonitorInstanceObj config)
    {
        InstanceUtils.SaveConfig(obj, config, ConfigName);
    }
    public static void SaveConfig()
    {
        ConfigSave.AddItem(new()
        {
            Name = "coloryr.monitor.config",
            Local = s_local,
            Obj = Config
        });
    }

    public static void ReadConfig()
    {
        Config = ConfigUtils.Config<MonitorConfigObj>(new()
        {
            Time = 1000
        }, s_local);
    }

    public static MonitorItemObj MakeNewItem()
    {
        return new()
        {
            Name = LangApi.GetLang("MonitorPlugin.Name1"),
            Display = MonitorDisplayType.Text,
            Margin = new MarginObj(5),
            Width = 0,
            Height = 0,
            Color1 = "#000000",
            Color2 = "#FFFFFF",
            Color3 = "#FFFFFF",
            FontSize = 30,
            Format = "{0}",
            Min = 0,
            Max = 100
        };
    }

    public static void UpdateSensor()
    {
        Computer.Accept(UpdateVisitor);
    }

    public static void Update(object? sender)
    {
        while (_run)
        {
            if (!_gethw)
            {
                UpdateSensor();
            }
            if (Config.Time > 20)
            {
                Thread.Sleep(Config.Time);
            }
            else
            {
                Thread.Sleep(1000);
            }
        }
    }

    public static IList<IHardware> GetHardwares()
    {
        _gethw = true;
        var list = Computer.Hardware;
        _gethw = false;
        return list;
    }

    public static Task WaitInit()
    {
        return Task.Run(() =>
        {
            while (!_run)
            {
                Thread.Sleep(200);
            }
        });
    }

    public static List<ISensor> GetSensors()
    {
        var list = new List<ISensor>();

        foreach (var hardware in GetHardwares())
        {
            foreach (var subhardware in hardware.SubHardware)
            {
                foreach (var sensor in subhardware.Sensors)
                {
                    list.Add(sensor);
                }
            }

            foreach (var sensor in hardware.Sensors)
            {
                list.Add(sensor);
            }
        }

        return list;
    }

    public bool CanCreateInstance => true;
    public bool HavePluginSetting => true;
    public bool HaveInstanceSetting => true;
    public bool CanEnable => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = LangApi.GetLang("MonitorPlugin.Name"),
            Plugin = "coloryr.monitor",
            Pos = PosEnum.TopRight,
            Margin = new(5)
        };
    }

    public void Disable()
    {

    }

    public void Enable()
    {

    }

    public Stream? GetIcon()
    {
        var assm = Assembly.GetExecutingAssembly();
        var item = assm.GetManifestResourceStream("ColorDesktop.MonitorPlugin.icon.png")!;
        return item;
    }

    public void Init(string local, string local1)
    {
        s_local = local + "/" + ConfigName;
        ReadConfig();

        Computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
            IsMotherboardEnabled = true,
            IsControllerEnabled = true,
            IsNetworkEnabled = true,
            IsStorageEnabled = true,
            IsBatteryEnabled = true,
            IsPsuEnabled = true
        };

        Task.Run(() =>
        {
            Computer.Open();

            _run = true;
            _timer = new(Update);
            _timer.Start();
        });
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new MonitorControl();
    }

    public Control OpenSetting(InstanceDataObj instance, bool isNew)
    {
        return new MonitorInstanceSettingControl(instance);
    }

    public Control OpenSetting()
    {
        return new MonitorSettingControl();
    }

    public void Stop()
    {
        Computer.Close();
        HidSelectorManager.Stop();
        _run = false;
    }

    public void LoadLang(LanguageType type)
    {
        var assm = Assembly.GetExecutingAssembly();
        if (assm == null)
        {
            return;
        }
        string name = type switch
        {
            LanguageType.en_us => "ColorDesktop.MonitorPlugin.Resource.Lang.en-us.json",
            _ => "ColorDesktop.MonitorPlugin.Resource.Lang.zh-cn.json"
        };
        using var item = assm.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(item);
        LangApi.AddLangs(reader.ReadToEnd());
    }

    public bool Permissions(string key, string permission)
    {
        return false;
    }
}
