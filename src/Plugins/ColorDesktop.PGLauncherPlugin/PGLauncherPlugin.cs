using System.Reflection;
using Avalonia.Controls;
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
        return InstanceUtils.GetConfig(obj, new PGLauncherInstanceObj()
        {
            Height = 300,
            Width = 150,
            PanelType = PanelType.Wrap,
            Items = []
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, PGLauncherInstanceObj config)
    {
        InstanceUtils.SaveConfig(obj, config, ConfigName);
    }

    public static void SaveConfig()
    {
        ConfigSave.AddItem(new()
        {
            Name = "coloryr.pglauncher.config",
            Local = s_local,
            Obj = Config
        });
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
        Config = ConfigUtils.Config<PGLauncherConfigObj>(new()
        {

        }, s_local);
    }

    public bool IsCoreLib => false;

    public bool HavePluginSetting => false;

    public bool HaveInstanceSetting => true;

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
        s_local = local + "/" + ConfigName;
        ReadConfig();
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new PGLauncherControl();
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        return new PGLauncherInstanceSettingControl(instance);
    }

    public Control OpenSetting()
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
