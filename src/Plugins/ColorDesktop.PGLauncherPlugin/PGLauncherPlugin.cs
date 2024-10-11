using System.Reflection;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using ColorDesktop.Api;
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
            Name = "新建项目",
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
            Nick = "程序启动器",
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

    public void Init(string local, string local1, LanguageType type)
    {
        s_local = local + "/" + ConfigName;


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
}
