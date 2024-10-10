using System.Reflection;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using ColorDesktop.Api;

namespace ColorDesktop.BmPlugin;

public class BmPlugin : IPlugin
{
    public bool IsCoreLib => false;

    public bool HavePluginSetting => false;

    public bool HaveInstanceSetting => false;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = "季度番剧",
            Plugin = "coloryr.bm",
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
        using var item = assm.GetManifestResourceStream("ColorDesktop.BmPlugin.icon.png")!;
        return new Bitmap(item);
    }

    public void Init(string local, string local1, LanguageType type)
    {
        
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new BmControl();
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        return new();
    }

    public Control OpenSetting()
    {
        return new();
    }

    public void Stop()
    {
        
    }
}
