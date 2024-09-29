using System.Reflection;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using ColorDesktop.Api;
using ColorDesktop.CoreLib;
using Lunar;

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

    public bool HavePluginSetting => true;

    public bool HaveInstanceSetting => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        throw new NotImplementedException();
    }

    public void Disable()
    {
        
    }

    public void Enable()
    {

    }

    public Bitmap? GetIcon()
    {
        var assm = Assembly.GetExecutingAssembly();
        using var item = assm.GetManifestResourceStream("ColorDesktop.CalendarPlugin.icon.png")!;
        return new Bitmap(item);
    }

    public void Init(string local, string local1, LanguageType type)
    {
        s_local = local + "/" + ConfigName;

        ReadConfig();
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        throw new NotImplementedException();
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        return new();
    }

    public Control OpenSetting()
    {
        return new();
    }

    public void Stop()
    {
        
    }
}
