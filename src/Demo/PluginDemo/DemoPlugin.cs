using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;

namespace PluginDemo;

//这是来自corelib的方法
public static class InstanceUtils
{
    public static string Local;
    public static string InstanceLocal;

    private static readonly Dictionary<string, object> s_configs = [];

    public static T GetConfig<T>(InstanceDataObj obj, T config, string name) where T : new()
    {
        if (s_configs.TryGetValue(obj.UUID, out var data))
        {
            return (T)data;
        }

        var obj1 = ConfigUtils.Config<T>(config, InstanceLocal + "/" + obj.UUID + "/" + name);

        obj1 ??= config;

        s_configs.Add(obj.UUID, obj1!);

        return obj1;
    }

    public static void SaveConfig(InstanceDataObj obj, object config, string name)
    {
        if (!s_configs.TryAdd(obj.UUID, config))
        {
            s_configs[obj.UUID] = config;
        }

        ConfigSave.AddItem(new()
        {
            Name = "demo" + name + obj.UUID,
            Local = InstanceLocal + "/" + obj.UUID + "/" + name,
            Obj = config
        });
    }
}


public class DemoPlugin : IPlugin
{
    public const string ConfigName = "demo.json";

    public static DemoObj Config;

    private static string s_local;

    public static DemoObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new DemoObj()
        {
            Text = "123"
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, DemoObj config)
    {
        InstanceUtils.SaveConfig(obj, config, ConfigName);
    }

    public static void SaveConfig()
    {
        ConfigSave.AddItem(new()
        {
            Name = "coloryr.demo.config",
            Local = s_local,
            Obj = Config
        });
    }

    public static void ReadConfig()
    {
        Config = ConfigUtils.Config<DemoObj>(new()
        {
            Text = "123"
        }, s_local);
    }

    public bool CanCreateInstance => false;

    public bool HavePluginSetting => true;

    public bool HaveInstanceSetting => true;

    public bool CanEnable => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Plugin = "coloryr.demo",
            Nick = "demo",
            Margin = new(5),
            Pos = PosEnum.TopLeft
        };
    }

    public void Disable()
    {
        Console.WriteLine("Demo plugin disable");
    }

    public void Enable()
    {
        Console.WriteLine("Demo plugin enable");
    }

    public Stream? GetIcon()
    {
        return null;
    }

    public void Init(string local, string instancelocal)
    {
        Console.WriteLine("Demo plugin init");

        s_local = local + "/" + ConfigName;

        InstanceUtils.Local = local;
        InstanceUtils.InstanceLocal = instancelocal;

        ReadConfig();
    }

    public void LoadLang(LanguageType type)
    {

    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new DemoControl();
    }

    public Control OpenSetting(InstanceDataObj instance, bool isNew)
    {
        return new DemoInstanceSettingControl(instance);
    }

    public Control OpenSetting()
    {
        return new DemoSettingControl();
    }

    public bool Permissions(string key, string permission)
    {
        return false;
    }

    public void Stop()
    {
        Console.WriteLine("Demo plugin stop");
    }
}
