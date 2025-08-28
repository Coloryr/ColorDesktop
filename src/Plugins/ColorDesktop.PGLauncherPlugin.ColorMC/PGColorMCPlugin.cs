using System.Reflection;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;

namespace ColorDesktop.PGLauncherPlugin.ColorMC;

public class PGColorMCPlugin : IPlugin
{
    public static PGColorMCConfigObj Config { get; set; }

    public const string ConfigName = "pglauncher.json";

    private static string s_local;

    public static PGColorMCInstanceObj GetConfig(InstanceDataObj obj)
    {
        return obj.GetConfig(new PGColorMCInstanceObj()
        {
            Height = 300,
            Width = 200,
            Display = DisplayType.NameIcon,
            BackColor = "#000000",
            TextColor = "#FFFFFF"
        }, ConfigName, JsonGen1.Default.PGColorMCInstanceObj);
    }

    public static void SaveConfig(InstanceDataObj obj, PGColorMCInstanceObj config)
    {
        obj.SaveConfig(config, ConfigName, JsonGen1.Default.PGColorMCInstanceObj);
    }

    public static void SaveConfig()
    {
        ConfigSave.AddItem(ConfigSaveObj.Build("coloryr.pglauncher.colormc.config", s_local, Config, JsonGen1.Default.PGColorMCConfigObj));
    }

    public static void ReadConfig()
    {
        Config = ConfigUtils.Config(new PGColorMCConfigObj()
        {

        }, s_local, JsonGen1.Default.PGColorMCConfigObj);
    }

    public bool CanCreateInstance => true;
    public bool HavePluginSetting => true;
    public bool HaveInstanceSetting => true;
    public bool CanEnable => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = LangApi.GetLang("PGColorMCPlugin.Name"),
            Plugin = "coloryr.pglaunher.colormc",
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
        var item = assm.GetManifestResourceStream("ColorDesktop.PGLauncherPlugin.ColorMC.icon.png")!;
        return item;
    }

    public void Init(string local, string local1)
    {
        s_local = Path.GetFullPath(local + "/" + ConfigName);

        ReadConfig();
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new PGColorMCControl();
    }

    public InstanceSetting OpenSetting(InstanceDataObj instance, bool isNew)
    {
        return new() { Control = new PGColorMCInstanceSettingControl(instance) };
    }

    public InstanceSetting OpenSetting()
    {
        return new() { Control = new PGColorMCSettingControl() };
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
            LanguageType.en_us => "ColorDesktop.PGLauncherPlugin.ColorMC.Resource.Lang.en-us.json",
            _ => "ColorDesktop.PGLauncherPlugin.ColorMC.Resource.Lang.zh-cn.json"
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
