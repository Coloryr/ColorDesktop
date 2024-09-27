using System.Reflection;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using ColorDesktop.Api;
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

    public bool IsCoreLib => false;

    public bool HavePluginSetting => true;

    public bool HaveInstanceSetting => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = "天气查询",
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

    public Bitmap? GetIcon()
    {
        var assm = Assembly.GetExecutingAssembly();
        using var item = assm.GetManifestResourceStream("ColorDesktop.WeatherPlugin.icon.png")!;
        return new Bitmap(item);
    }

    public void Init(string local, string local1, LanguageType type)
    {
        s_local = local + "/" + ConfigName;

        var assm = Assembly.GetExecutingAssembly();
        var jsonSerializer = JsonSerializer.CreateDefault();
        {
            using var item = assm.GetManifestResourceStream("ColorDesktop.WeatherPlugin.Resource.City.json")!;
            using var reader = new JsonTextReader(new StreamReader(item));
            YuApi.Citys = jsonSerializer.Deserialize<List<CityObj>>(reader)!;
        }

        {
            using var item1 = assm.GetManifestResourceStream("ColorDesktop.WeatherPlugin.Resource.City1.json")!;
            using var reader1 = new JsonTextReader(new StreamReader(item1));
            AmapApi.Citys = jsonSerializer.Deserialize<List<City1Obj>>(reader1)!;
        }

        ReadConfig();
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new WeatherControl();
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        return new WeatherInstanceSettingControl(instance);
    }

    public Control OpenSetting()
    {
        return new WeatherSettingControl();
    }

    public void Stop()
    {

    }
}
