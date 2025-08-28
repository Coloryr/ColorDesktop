using System.Reflection;
using Avalonia.Media.Imaging;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;

namespace ColorDesktop.BmPlugin;

public class BmPlugin : IPlugin
{
    public const string ConfigName = "bm.json";

    public static Bitmap Icon { get; private set; }
    public static Bitmap Icon1 { get; private set; }

    public static BmInstanceObj GetConfig(InstanceDataObj obj)
    {
        return obj.GetConfig(new BmInstanceObj()
        {
            Skin = SkinType.Skin1,
            Width = 280,
            Height = 0
        }, ConfigName, JsonGen.Default.BmInstanceObj);
    }

    public static void SaveConfig(InstanceDataObj obj, BmInstanceObj config)
    {
        obj.SaveConfig(config, ConfigName, JsonGen.Default.BmInstanceObj);
    }

    public bool CanCreateInstance => true;

    public bool HavePluginSetting => false;

    public bool HaveInstanceSetting => true;

    public bool CanEnable => true;

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
        var assm = Assembly.GetExecutingAssembly();
        {
            using var item = assm.GetManifestResourceStream("ColorDesktop.BmPlugin.Resource.image1.png")!;
            Icon = new Bitmap(item);
        }
        {
            using var item = assm.GetManifestResourceStream("ColorDesktop.BmPlugin.Resource.image2.png")!;
            Icon1 = new Bitmap(item);
        }
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

    public InstanceSetting OpenSetting(InstanceDataObj instance, bool isNew)
    {
        return new() { Control = new BmInstanceSettingControl(instance) };
    }

    public InstanceSetting OpenSetting()
    {
        return new();
    }

    public bool Permissions(string key, string permission)
    {
        return false;
    }

    public void Stop()
    {

    }
}
