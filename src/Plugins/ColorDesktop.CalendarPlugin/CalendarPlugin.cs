using System.Reflection;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.CoreLib;

namespace ColorDesktop.CalendarPlugin;

public class CalendarPlugin : IPlugin
{
    public static CalendarConfigObj Config { get; set; }

    public const string ConfigName = "calendar.json";

    private static string s_local;

    public static CalendarInstanceObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new CalendarInstanceObj()
        {
            BackColor = "#000000",
            TextColor = "#FFFFFF"
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, CalendarInstanceObj config)
    {
        InstanceUtils.SaveConfig(obj, config, ConfigName);
    }

    public static void SaveConfig()
    {
        ConfigSave.AddItem(new()
        {
            Name = "coloryr.calendar.config",
            Local = s_local,
            Obj = Config
        });
    }

    public static void ReadConfig()
    {
        Config = ConfigUtils.Config<CalendarConfigObj>(new()
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
            Nick = LangApi.GetLang("CalendarPlugin.Name"),
            Plugin = "coloryr.calendar",
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
        var item = assm.GetManifestResourceStream("ColorDesktop.CalendarPlugin.icon.png")!;
        return item;
    }

    public void Init(string local, string local1)
    {
        s_local = local + "/" + ConfigName;

        ReadConfig();
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new CalendarControl();
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        return new CalendarInstanceSettingControl(instance);
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
            LanguageType.en_us => "ColorDesktop.CalendarPlugin.Resource.Lang.en-us.json",
            _ => "ColorDesktop.CalendarPlugin.Resource.Lang.zh-cn.json"
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
