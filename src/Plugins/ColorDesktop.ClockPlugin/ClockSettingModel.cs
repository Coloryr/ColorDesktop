using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.ClockPlugin;

public partial class ClockSettingModel : ObservableObject
{
    [ObservableProperty]
    private bool _ntp;

    [ObservableProperty]
    private string _ntpIp;

    [ObservableProperty]
    private string _ntpTest;

    [ObservableProperty]
    private int _updateTime;
    [ObservableProperty]
    private int _timeZone;

    public ClockSettingModel()
    {
        var config = ClockPlugin.Config;
        if (config == null)
        {
            return;
        }

        Ntp = config.Ntp;
        NtpIp = config.NtpIp;
        UpdateTime = config.NtpUpdateTime;
        TimeZone = config.TimeZone;
    }

    partial void OnTimeZoneChanged(int value)
    {
        ClockPlugin.Config.TimeZone = TimeZone;
        ClockPlugin.SaveConfig();
    }

    partial void OnNtpChanged(bool value)
    {
        ClockPlugin.Config.Ntp = value;
        ClockPlugin.SaveConfig();
    }

    partial void OnNtpIpChanged(string value)
    {
        ClockPlugin.Config.NtpIp = value;
        ClockPlugin.SaveConfig();
    }

    partial void OnUpdateTimeChanged(int value)
    {
        ClockPlugin.Config.NtpUpdateTime = value;
        ClockPlugin.SaveConfig();
    }

    [RelayCommand]
    public async Task TestNtp()
    {
        if (await NtpClient.GetTime())
        {
            NtpTest = NtpClient.Date.ToString();
        }
        else
        {
            NtpTest = "NTP服务器错误";
        }
    }
}
