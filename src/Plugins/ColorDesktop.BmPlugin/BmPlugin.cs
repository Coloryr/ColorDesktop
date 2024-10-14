using System.Reflection;
using Avalonia.Controls;
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
            Nick = LangApi.GetLang("BmPlugin.Name"),
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

    public Stream? GetIcon()
    {
        var assm = Assembly.GetExecutingAssembly();
        var item = assm.GetManifestResourceStream("ColorDesktop.BmPlugin.icon.png")!;
        return item;
    }

    public void Init(string local, string local1)
    {
        
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
            LanguageType.en_us => "ColorDesktop.BmPlugin.Resource.Lang.en-us.json",
            _ => "ColorDesktop.BmPlugin.Resource.Lang.zh-cn.json"
        };
        using var item = assm.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(item);
        LangApi.AddLangs(reader.ReadToEnd());
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
