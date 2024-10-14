using System.Reflection;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.CoreLib;

namespace ColorDesktop.ClockPlugin;

public class ClockPlugin : IPlugin
{
    public static ClockConfigObj Config { get; set; }

    public const string ConfigName = "clock.json";

    private static string s_local;

    public static ClockInstanceObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new ClockInstanceObj()
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
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, ClockInstanceObj config)
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
        Config = ConfigUtils.Config<ClockConfigObj>(new()
        {
            NtpIp = "cn.pool.ntp.org",
            NtpUpdateTime = 180,
            TimeZone = 8
        }, s_local);
    }

    public bool HavePluginSetting => true;

    public bool HaveInstanceSetting => true;

    public bool IsCoreLib => false;

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
        s_local = local + "/" + ConfigName;
        ReadConfig();
        NtpClient.Start();
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new ClockControl();
    }

    public Control OpenSetting(InstanceDataObj obj)
    {
        return new ClockInstanceSettingControl(obj);
    }

    public Control OpenSetting()
    {
        return new ClockSettingControl();
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
}
