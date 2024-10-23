using System.Reflection;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.CoreLib;

namespace ColorDesktop.MinecraftMotdPlugin;

public class MinecraftMotdPlugin : IPlugin
{
    public const string ConfigName = "skin.json";

    public static MotdInstanceObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new MotdInstanceObj()
        {

        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, MotdInstanceObj config)
    {
        InstanceUtils.SaveConfig(obj, config, ConfigName);
    }

    public bool IsCoreLib => false;

    public bool HavePluginSetting => false;

    public bool HaveInstanceSetting => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = LangApi.GetLang("MinecraftMotdPlugin.Name"),
            Plugin = "coloryr.minecraft.motd",
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
        var item = assm.GetManifestResourceStream("ColorDesktop.MinecraftMotdPlugin.icon.png")!;
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
            LanguageType.en_us => "ColorDesktop.MinecraftMotdPlugin.Resource.Lang.en-us.json",
            _ => "ColorDesktop.MinecraftMotdPlugin.Resource.Lang.zh-cn.json"
        };
        using var item = assm.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(item);
        LangApi.AddLangs(reader.ReadToEnd());
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new MinecraftMotdControl();
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        return new MotdInstanceSettingControl(instance);
    }

    public Control OpenSetting()
    {
        return new();
    }

    public void Stop()
    {
        
    }
}
