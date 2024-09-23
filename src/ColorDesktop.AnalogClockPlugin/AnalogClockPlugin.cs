using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using ColorDesktop.Api;

namespace ColorDesktop.AnalogClockPlugin;

public class AnalogClockPlugin : IPlugin
{
    public static AnalogClockConfigObj Config { get; set; }

    public const string ConfigName = "analogclock.json";

    public static string Local;
    public static string InstanceLocal;

    private static Dictionary<string, AnalogClockConfigObj> s_configs = [];

    public static AnalogClockConfigObj GetConfig(InstanceDataObj obj)
    {
        if (s_configs.TryGetValue(obj.UUID, out var data))
        {
            return data;
        }

        var obj1 = ConfigUtils.Config<AnalogClockConfigObj>(new()
        {
            Size = 100,
            Type = ClockType.Analog,
            Color = "Red"
        }, InstanceLocal + "/" + obj.UUID + "/" + ConfigName);

        s_configs.Add(obj.UUID, obj1);

        return obj1;
    }

    public static void SaveConfig(InstanceDataObj obj, AnalogClockConfigObj config)
    {
        if (!s_configs.TryAdd(obj.UUID, config))
        {
            s_configs[obj.UUID] = config;
        }

        ConfigSave.AddItem(new()
        {
            Name = "coloryr.analogclock.config" + obj.UUID,
            Local = InstanceLocal + "/" + obj.UUID + "/" + ConfigName,
            Obj = config
        });
    }

    public bool HavePluginSetting => false;

    public bool HaveInstanceSetting => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = "模拟时钟",
            Plugin = "coloryr.analogclock",
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

    public Bitmap? GetIcon()
    {
        var assm = Assembly.GetExecutingAssembly();
        using var item = assm.GetManifestResourceStream("ColorDesktop.AnalogClockPlugin.icon.png")!;
        return new Bitmap(item);
    }

    public void Init(string local, string local1, LanguageType type)
    {
        Local = local + "/" + ConfigName;
        InstanceLocal = local1;
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        var control = new AnalogClockControl();
        control.Update(obj);
        return control;
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        return new AnalogClockSettingControl(instance);
    }

    public Control OpenSetting()
    {
        return new();
    }

    public void Stop()
    {
        
    }
}
