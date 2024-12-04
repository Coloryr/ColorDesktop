using System.Reflection;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;
using MinecraftSkinRender;

namespace ColorDesktop.MinecraftSkinPlugin;

public class MinecraftSkinPlugin : IPlugin
{
    public const string ConfigName = "skin.json";

    public static SkinInstanceObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new SkinInstanceObj()
        {
            Width = 300,
            Height = 300,
            File = "color_yr",
            FileType = FileType.Name,
            SkinType = SkinType.NewSlim,
            EnableCape = true,
            EnableTop = true,
            EnableAnimation = true
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, SkinInstanceObj config)
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
            Nick = LangApi.GetLang("MinecraftSkinPlugin.Name"),
            Plugin = "coloryr.minecraft.skin",
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
        var item = assm.GetManifestResourceStream("ColorDesktop.MinecraftSkinPlugin.icon.png")!;
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
            LanguageType.en_us => "ColorDesktop.MinecraftSkinPlugin.Resource.Lang.en-us.json",
            _ => "ColorDesktop.MinecraftSkinPlugin.Resource.Lang.zh-cn.json"
        };
        using var item = assm.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(item);
        LangApi.AddLangs(reader.ReadToEnd());
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new MinecraftSkinControl();
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        return new SkinInstanceSettingControl(instance);
    }

    public Control OpenSetting()
    {
        return new();
    }

    public void Stop()
    {

    }

    public bool Permissions(string key, string permission)
    {
        return false;
    }
}
