using System.Reflection;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;

namespace ColorDesktop.OneWordPlugin;

public class OneWordPlugin : IPlugin
{
    public const string ConfigName = "oneword.json";

    public static OneWordInstanceObj GetConfig(InstanceDataObj obj)
    {
        return obj.GetConfig(new OneWordInstanceObj()
        {
            Size = 80,
            TextColor = "#FFFFFF",
            BackColor = "#000000",
            Width = 600
        }, ConfigName, JsonGen.Default.OneWordInstanceObj);
    }

    public static void SaveConfig(InstanceDataObj obj, OneWordInstanceObj config)
    {
        obj.SaveConfig(config, ConfigName, JsonGen.Default.OneWordInstanceObj);
    }

    public bool CanCreateInstance => true;
    public bool HavePluginSetting => false;
    public bool HaveInstanceSetting => true;
    public bool CanEnable => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = LangApi.GetLang("OneWordPlugin.Name"),
            Plugin = "coloryr.oneword",
            Pos = PosEnum.Top,
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
        var item = assm.GetManifestResourceStream("ColorDesktop.OneWordPlugin.icon.png")!;
        return item;
    }

    public void Init(string local, string local1)
    {

    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new OneWordControl();
    }

    public InstanceSetting OpenSetting(InstanceDataObj instance, bool isNew)
    {
        return new() { Control = new OneWordInstanceSettingControl(instance) };
    }

    public InstanceSetting OpenSetting()
    {
        return new();
    }

    public void Stop()
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
            LanguageType.en_us => "ColorDesktop.OneWordPlugin.Resource.Lang.en-us.json",
            _ => "ColorDesktop.OneWordPlugin.Resource.Lang.zh-cn.json"
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
