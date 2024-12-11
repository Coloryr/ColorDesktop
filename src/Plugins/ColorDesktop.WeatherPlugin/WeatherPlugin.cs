using System.Reflection;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;
using ColorDesktop.WeatherPlugin.Objs;
using Newtonsoft.Json;

namespace ColorDesktop.WeatherPlugin;

public class WeatherPlugin : IPlugin
{
    public const string ConfigName = "weather.json";

    public static WeatherConfigObj Config;

    private static string s_local;

    public static WeatherInstanceObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new WeatherInstanceObj()
        {
            City = "110000",
            BackColor = "#57000000",
            TextColor = "#FFFFFF"
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, WeatherInstanceObj config)
    {
        InstanceUtils.SaveConfig(obj, config, ConfigName);
    }

    public static void SaveConfig()
    {
        ConfigSave.AddItem(new()
        {
            Name = "coloryr.clock.config",
            Local = s_local,
            Obj = Config
        });
    }

    public static void ReadConfig()
    {
        Config = ConfigUtils.Config<WeatherConfigObj>(new()
        {
            AutoUpdate = true,
            UpdateTime = 360
        }, s_local);
    }

    public bool CanCreateInstance => true;
    public bool HavePluginSetting => true;
    public bool HaveInstanceSetting => true;
    public bool CanEnable => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = LangApi.GetLang("WeatherPlugin.Name"),
            Plugin = "coloryr.weather",
            Pos = PosEnum.TopLeft,
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
        var item = assm.GetManifestResourceStream("ColorDesktop.WeatherPlugin.icon.png")!;
        return item;
    }

    public void Init(string local, string local1)
    {
        s_local = local + "/" + ConfigName;

        var assm = Assembly.GetExecutingAssembly();
        var jsonSerializer = JsonSerializer.CreateDefault();
        using var item1 = assm.GetManifestResourceStream("ColorDesktop.WeatherPlugin.Resource.City1.json")!;
        using var reader1 = new JsonTextReader(new StreamReader(item1));
        AmapApi.Citys = jsonSerializer.Deserialize<List<City1Obj>>(reader1)!;

        ReadConfig();
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new WeatherControl();
    }

    public InstanceSetting OpenSetting(InstanceDataObj instance, bool isNew)
    {
        return new() { Control = new WeatherInstanceSettingControl(instance) };
    }

    public InstanceSetting OpenSetting()
    {
        return new() { Control = new WeatherSettingControl() };
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
            LanguageType.en_us => "ColorDesktop.WeatherPlugin.Resource.Lang.en-us.json",
            _ => "ColorDesktop.WeatherPlugin.Resource.Lang.zh-cn.json"
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
