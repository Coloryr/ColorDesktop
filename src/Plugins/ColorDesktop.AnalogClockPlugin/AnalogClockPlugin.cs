using System.Reflection;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.CoreLib;

namespace ColorDesktop.AnalogClockPlugin;

public class AnalogClockPlugin : IPlugin
{
    public const string ConfigName = "analogclock.json";

    public static AnalogClockInstanceConfigObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new AnalogClockInstanceConfigObj()
        {
            Size = 100,
            Type = ClockType.Analog,
            Color = "Red",
            TextSize = 120,
            TextColor = "#FFFFFF",
            BackColor = "#0000CD",
            BorderColor = "#EEEEEE"
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, AnalogClockInstanceConfigObj config)
    {
        InstanceUtils.SaveConfig(obj, config, ConfigName);
    }

    public bool HavePluginSetting => false;

    public bool HaveInstanceSetting => true;

    public bool IsCoreLib => false;

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

    public Stream? GetIcon()
    {
        var assm = Assembly.GetExecutingAssembly();
        var item = assm.GetManifestResourceStream("ColorDesktop.AnalogClockPlugin.icon.png")!;
        return item;
    }

    public void Init(string local, string local1, LanguageType type)
    {

    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new AnalogClockControl();
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
