using System.Reflection;
using System.Text.Json.Serialization.Metadata;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;

namespace PluginDemo;

//这是来自corelib的方法
public static class InstanceUtils
{
    public static string Local;
    public static string InstanceLocal;

    private static readonly Dictionary<string, object> s_configs = [];

    public static T GetConfig<T>(this InstanceDataObj obj, T config, string name, JsonTypeInfo<T> info) where T : new()
    {
        if (s_configs.TryGetValue(obj.UUID, out var data))
        {
            return (T)data!;
        }

        var obj1 = ConfigUtils.Config<T>(config, Path.GetFullPath(InstanceLocal + "/" + obj.UUID + "/" + name), info);

        obj1 ??= config;

        s_configs.Add(obj.UUID, obj1!);

        return obj1;
    }

    public static void SaveConfig<T>(this InstanceDataObj obj, T config, string name, JsonTypeInfo<T> info)
    {
        if (!s_configs.TryAdd(obj.UUID, config!))
        {
            s_configs[obj.UUID] = config!;
        }

        ConfigSave.AddItem(ConfigSaveObj.Build("corelib" + name + obj.UUID,
            Path.GetFullPath(InstanceLocal + "/" + obj.UUID + "/" + name), config, info));
    }
}

public class DemoPlugin : IPlugin
{
    public const string ConfigName = "demo.json";

    public static DemoObj Config;

    private static string s_local;

    public static DemoObj GetConfig(InstanceDataObj obj)
    {
        return obj.GetConfig(new DemoObj()
        {
            Text = "123"
        }, ConfigName, JsonGen.Default.DemoObj);
    }

    public static void SaveConfig(InstanceDataObj obj, DemoObj config)
    {
        obj.SaveConfig(config, ConfigName, JsonGen.Default.DemoObj);
    }

    public static void SaveConfig()
    {
        ConfigSave.AddItem(ConfigSaveObj.Build("coloryr.demo.config", s_local, Config, JsonGen.Default.DemoObj));
    }

    public static void ReadConfig()
    {
        Config = ConfigUtils.Config(new DemoObj()
        {
            Text = "123"
        }, s_local, JsonGen.Default.DemoObj);
    }

    public bool CanCreateInstance => true;

    public bool HavePluginSetting => true;

    public bool HaveInstanceSetting => true;

    public bool CanEnable => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        Console.WriteLine("Demo plugin create instance config");

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
        Console.WriteLine("Demo plugin get icon");

        return null;
    }

    public void Init(string local, string instancelocal)
    {
        Console.WriteLine("Demo plugin init");

        s_local = Path.GetFullPath(local + "/" + ConfigName);

        InstanceUtils.Local = local;
        InstanceUtils.InstanceLocal = instancelocal;

        ReadConfig();
    }

    public void LoadLang(LanguageType type)
    {
        Console.WriteLine("Demo plugin load lang");

        var assm = Assembly.GetExecutingAssembly();
        if (assm == null)
        {
            return;
        }
        string name = type switch
        {
            LanguageType.en_us => "PluginDemo.Resource.Lang.en-us.json",
            _ => "PluginDemo.Resource.Lang.zh-cn.json"
        };
        using var item = assm.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(item);
        LangApi.AddLangs(reader.ReadToEnd());
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        Console.WriteLine("Demo plugin make instance");

        return new DemoControl();
    }

    public InstanceSetting OpenSetting(InstanceDataObj instance, bool isNew)
    {
        Console.WriteLine("Demo plugin open setting with instance");

        return new() { Control = new DemoInstanceSettingControl(instance) };
    }

    public InstanceSetting OpenSetting()
    {
        Console.WriteLine("Demo plugin open setting");

        return new() { Control = new DemoSettingControl() };
    }

    public bool Permissions(string key, string permission)
    {
        Console.WriteLine("Demo plugin check permission");

        return false;
    }

    public void Stop()
    {
        Console.WriteLine("Demo plugin stop");
    }
}
