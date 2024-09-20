using System.Reflection;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using ColorDesktop.Api;

namespace ColorDesktop.ClockPlugin;

public class ClockPlugin : IPlugin
{
    public bool HavePluginSetting => true;

    public bool HaveInstanceSetting => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = "桌面时钟",
            Plugin = "coloryr.clock",
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
        using var item = assm.GetManifestResourceStream("ColorDesktop.ClockPlugin.icon.png")!;
        return new Bitmap(item);
    }

    public void Init(string local, LanguageType type)
    {

    }

    public IInstance MakeInstances(string local, InstanceDataObj arg)
    {
        return new ClockControl();
    }

    public void Stop()
    {
        
    }

    Control IPlugin.OpenSetting(InstanceDataObj instance)
    {
        return new ClockSettingControl();
    }

    Control IPlugin.OpenSetting()
    {
        return new ClockSettingControl();
    }
}
