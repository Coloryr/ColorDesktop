using System.Reflection;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;

namespace ColorDesktop.PGLauncherPlugin;

public class PGLauncherPlugin : IPlugin
{
    public static PGLauncherConfigObj Config { get; set; }

    public const string ConfigName = "pglauncher.json";

    private static string s_local;

    public static PGLauncherInstanceObj GetConfig(InstanceDataObj obj)
    {
        return obj.GetConfig(new PGLauncherInstanceObj()
        {
            Height = 300,
            Width = 150,
            PanelType = PanelType.Wrap,
            Items = []
        }, ConfigName, JsonGen.Default.PGLauncherInstanceObj);
    }

    public static void SaveConfig(InstanceDataObj obj, PGLauncherInstanceObj config)
    {
        obj.SaveConfig(config, ConfigName, JsonGen.Default.PGLauncherInstanceObj);
    }

    public static void SaveConfig()
    {
        ConfigSave.AddItem(ConfigSaveObj.Build("coloryr.pglauncher.config", s_local, Config, JsonGen.Default.PGLauncherConfigObj));
    }

    public static PGItemObj MakeNewItem()
    {
        return new()
        {
            Name = LangApi.GetLang("PGLauncherPlugin.Name1"),
            Display = DisplayType.Text,
            Margin = new MarginObj(5),
            Size = 30,
            BackColor = "#000000",
            TextColor = "#FFFFFF",
            BorderSize = 5,
            TextSize = 15
        };
    }

    public static void ReadConfig()
    {
        Config = ConfigUtils.Config(new PGLauncherConfigObj()
        {

        }, s_local, JsonGen.Default.PGLauncherConfigObj);
    }

    public bool CanCreateInstance => true;
    public bool HavePluginSetting => false;
    public bool HaveInstanceSetting => true;
    public bool CanEnable => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = LangApi.GetLang("PGLauncherPlugin.Name"),
            Plugin = "coloryr.pglaunher",
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
        var item = assm.GetManifestResourceStream("ColorDesktop.PGLauncherPlugin.icon.png")!;
        return item;
    }

    public void Init(string local, string local1)
    {
        s_local = Path.GetFullPath(local + "/" + ConfigName);
        ReadConfig();
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new PGLauncherControl();
    }

    public InstanceSetting OpenSetting(InstanceDataObj instance, bool isNew)
    {
        return new() { Control = new PGLauncherInstanceSettingControl(instance) };
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
            LanguageType.en_us => "ColorDesktop.PGLauncherPlugin.Resource.Lang.en-us.json",
            _ => "ColorDesktop.PGLauncherPlugin.Resource.Lang.zh-cn.json"
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
