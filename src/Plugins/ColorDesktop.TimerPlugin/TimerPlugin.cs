using System.Reflection;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;

namespace ColorDesktop.TimerPlugin;

public class TimerPlugin : IPlugin
{
    public const string ConfigName = "timer.json";

    public static TimerInstanceObj GetConfig(InstanceDataObj obj)
    {
        return obj.GetConfig(new TimerInstanceObj()
        {
            MaxHeight = 400,
            Timers = []
        }, ConfigName, JsonGen.Default.TimerInstanceObj);
    }

    public static void SaveConfig(InstanceDataObj obj, TimerInstanceObj config)
    {
        obj.SaveConfig(config, ConfigName, JsonGen.Default.TimerInstanceObj);
    }

    public bool CanEnable => true;

    public bool CanCreateInstance => true;

    public bool HavePluginSetting => false;

    public bool HaveInstanceSetting => false;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = LangApi.GetLang("TimerPlugin.Name"),
            Plugin = "coloryr.timer",
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
        return null;
    }

    public void Init(string local, string instancelocal)
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
            LanguageType.en_us => "ColorDesktop.TimerPlugin.Resource.Lang.en-us.json",
            _ => "ColorDesktop.TimerPlugin.Resource.Lang.zh-cn.json"
        };
        using var item = assm.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(item);
        LangApi.AddLangs(reader.ReadToEnd());
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new TimerControl(obj);
    }

    public InstanceSetting OpenSetting(InstanceDataObj instance, bool isNew)
    {
        return new();
    }

    public InstanceSetting OpenSetting()
    {
        return new();
    }

    public bool Permissions(string key, string permission)
    {
        return true;
    }

    public void Stop()
    {

    }
}
