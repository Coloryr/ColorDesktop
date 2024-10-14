using System.Reflection;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.CoreLib;

namespace ColorDesktop.PGLauncherPlugin.ColorMC;

public class PGColorMCPlugin : IPlugin
{
    public static PGColorMCConfigObj Config { get; set; }

    public const string ConfigName = "pglauncher.json";

    private static string s_local;

    public static PGColorMCInstanceObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new PGColorMCInstanceObj()
        {
            Height = 300,
            Width = 200,
            Display = DisplayType.NameIcon,
            BackColor = "#000000",
            TextColor = "#FFFFFF"
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, PGColorMCInstanceObj config)
    {
        InstanceUtils.SaveConfig(obj, config, ConfigName);
    }

    public static void SaveConfig()
    {
        ConfigSave.AddItem(new()
        {
            Name = "coloryr.pglauncher.colormc.config",
            Local = s_local,
            Obj = Config
        });
    }

    public static void ReadConfig()
    {
        Config = ConfigUtils.Config<PGColorMCConfigObj>(new()
        {

        }, s_local);
    }

    public bool IsCoreLib => false;

    public bool HavePluginSetting => true;

    public bool HaveInstanceSetting => true;

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
        s_local = local + "/" + ConfigName;

        ReadConfig();
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new PGColorMCControl();
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        return new PGColorMCInstanceSettingControl(instance);
    }

    public Control OpenSetting()
    {
        return new PGColorMCSettingControl();
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
}
