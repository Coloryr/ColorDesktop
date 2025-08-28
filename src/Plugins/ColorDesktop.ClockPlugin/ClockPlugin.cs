using System.Reflection;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;

namespace ColorDesktop.ClockPlugin;

public class ClockPlugin : IPlugin
{
    public static ClockConfigObj Config { get; set; }

    public const string ConfigName = "clock.json";

    private static string s_local;

    public static ClockInstanceObj GetConfig(InstanceDataObj obj)
    {
        return obj.GetConfig(new ClockInstanceObj()
        {
            Color = "#000000",
            Size = 50,
            HourColor = "#000000",
            MinuteColor = "#000000",
            SecondColor = "#000000",
            ColonColor = "#000000",
            HourSize = 50,
            MinuteSize = 50,
            SecondSize = 50,
            ColonSize = 50,
            BackGround = "#00000000"
        }, ConfigName, JsonGen.Default.ClockInstanceObj);
    }

    public static DateTime GetTime()
    {
        return Config.Ntp ? NtpClient.Date : DateTime.UtcNow.AddHours(Config.TimeZone);
    }

    public static void SaveConfig(InstanceDataObj obj, ClockInstanceObj config)
    {
        obj.SaveConfig(config, ConfigName, JsonGen.Default.ClockInstanceObj);
    }

    public static void SaveConfig()
    {
        ConfigSave.AddItem(ConfigSaveObj.Build("coloryr.clock.config", s_local, Config, JsonGen.Default.ClockConfigObj));
    }

    public static void ReadConfig()
    {
        Config = ConfigUtils.Config(new()
        {
            NtpIp = "cn.pool.ntp.org",
            NtpUpdateTime = 180,
            TimeZone = 8
        }, s_local, JsonGen.Default.ClockConfigObj);
    }

    public bool HavePluginSetting => true;
    public bool HaveInstanceSetting => true;
    public bool CanCreateInstance => true;
    public bool CanEnable => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = LangApi.GetLang("ClockPlugin.Name"),
            Plugin = "coloryr.clock",
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
        var item = assm.GetManifestResourceStream("ColorDesktop.ClockPlugin.icon.png")!;
        return item;
    }

    public void Init(string local, string local1)
    {
        s_local = Path.GetFullPath(local + "/" + ConfigName);
        ReadConfig();
        NtpClient.Start();
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new ClockControl();
    }

    public InstanceSetting OpenSetting(InstanceDataObj obj, bool isNew)
    {
        return new() { Control = new ClockInstanceSettingControl(obj) };
    }

    public InstanceSetting OpenSetting()
    {
        return new() { Control = new ClockSettingControl() };
    }

    public void Stop()
    {
        NtpClient.Stop();
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
            LanguageType.en_us => "ColorDesktop.ClockPlugin.Resource.Lang.en-us.json",
            _ => "ColorDesktop.ClockPlugin.Resource.Lang.zh-cn.json"
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
