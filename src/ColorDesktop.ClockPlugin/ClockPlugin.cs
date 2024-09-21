using System.Reflection;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using ColorDesktop.Api;

namespace ColorDesktop.ClockPlugin;

public class ClockPlugin : IPlugin
{
    public static ClockConfigObj Config { get; set; }

    public const string ConfigName = "clock.json";

    public static string Local;
    public static string InstanceLocal;

    public static void SaveConfig()
    {
        ConfigSave.AddItem(new()
        {
            Name = "coloryr.clock.config",
            Local = Local,
            Obj = Config
        });
    }

    public static ClockInstanceObj GetConfig(InstanceDataObj obj)
    {
        return ConfigUtils.Config<ClockInstanceObj>(new()
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
        }, InstanceLocal + "/" + obj.UUID + "/" + ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, ClockInstanceObj config)
    {
        ConfigSave.AddItem(new()
        {
            Name = "coloryr.clock.config" + obj.UUID,
            Local = InstanceLocal + "/" + obj.UUID + "/" + ConfigName,
            Obj = config
        });
    }

    public bool HavePluginSetting => true;

    public bool HaveInstanceSetting => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = "桌面时钟",
            Plugin = "coloryr.clock",
            Pos = PosEnum.TopRight,
            Margin = new(5)
        };
    }

    public void Disable()
    {
        NtpClient.Stop();
    }

    public void Enable()
    {
        NtpClient.Start();
    }

    public Bitmap? GetIcon()
    {
        var assm = Assembly.GetExecutingAssembly();
        using var item = assm.GetManifestResourceStream("ColorDesktop.ClockPlugin.icon.png")!;
        return new Bitmap(item);
    }

    public void Init(string local, string local1, LanguageType type)
    {
        Local = local + "/" + ConfigName;
        InstanceLocal = local1;
        Config = ConfigUtils.Config<ClockConfigObj>(new()
        {
            NtpIp = "cn.pool.ntp.org",
            NtpUpdateTime = 180,
            TimeZone = 8
        }, Local);
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        var control = new ClockControl();
        control.Update(obj);
        return control;
    }

    public void Stop()
    {

    }

    Control IPlugin.OpenSetting(InstanceDataObj obj)
    {
        return new ClockInstanceSettingControl(obj);
    }

    Control IPlugin.OpenSetting()
    {
        return new ClockSettingControl();
    }
}
