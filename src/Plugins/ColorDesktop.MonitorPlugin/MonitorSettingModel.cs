using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.MonitorPlugin;

public partial class MonitorSettingModel : ObservableObject
{
    [ObservableProperty]
    private int _time;

    public bool IsAdmin { get; init; }

    public MonitorSettingModel()
    {
        if (MonitorPlugin.Config is { } config)
        {
            _time = config.Time;
        }

        IsAdmin = MonitorPlugin.IsRunAsAdmin();
    }

    partial void OnTimeChanged(int value)
    {
        MonitorPlugin.Config.Time = value;
        MonitorPlugin.SaveConfig();
    }
}
