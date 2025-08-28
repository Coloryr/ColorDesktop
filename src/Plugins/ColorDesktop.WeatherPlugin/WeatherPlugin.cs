using System.Reflection;
using System.Text.Json;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;
using ColorDesktop.WeatherPlugin.Objs;

namespace ColorDesktop.WeatherPlugin;

public class WeatherPlugin : IPlugin
{
    public const string ConfigName = "weather.json";

    public static WeatherConfigObj Config;

    private static string s_local;

    public static WeatherInstanceObj GetConfig(InstanceDataObj obj)
    {
        return obj.GetConfig(new WeatherInstanceObj()
        {
            City = "110000",
            BackColor = "#57000000",
            TextColor = "#FFFFFF"
        }, ConfigName, JsonGen.Default.WeatherInstanceObj);
    }

    public static void SaveConfig(InstanceDataObj obj, WeatherInstanceObj config)
    {
        obj.SaveConfig(config, ConfigName, JsonGen.Default.WeatherInstanceObj);
    }

    public static void SaveConfig()
    {
        ConfigSave.AddItem(ConfigSaveObj.Build("coloryr.weather.config", s_local, Config, JsonGen.Default.WeatherConfigObj));
    }

    public static void ReadConfig()
    {
        Config = ConfigUtils.Config(new WeatherConfigObj()
        {
            AutoUpdate = true,
            UpdateTime = 360
        }, s_local, JsonGen.Default.WeatherConfigObj);
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
        s_local = Path.GetFullPath(local + "/" + ConfigName);

        var assm = Assembly.GetExecutingAssembly();
        using var item1 = assm.GetManifestResourceStream("ColorDesktop.WeatherPlugin.Resource.city.json")!;
        var jsonSerializer = JsonDocument.Parse(item1);
        ColorMCApi.Citys = jsonSerializer.Deserialize(JsonGen.Default.ListCity1Obj)!;

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
