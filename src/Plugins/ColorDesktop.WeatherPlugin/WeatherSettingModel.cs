using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.WeatherPlugin;

public partial class WeatherSettingModel : ObservableObject
{
    [ObservableProperty]
    private bool _autoUpdate;

    [ObservableProperty]
    private int? _time;

    public WeatherSettingModel()
    {
        if (WeatherPlugin.Config == null)
        {
            return;
        }

        _autoUpdate = WeatherPlugin.Config.AutoUpdate;
        _time = WeatherPlugin.Config.UpdateTime;
    }

    partial void OnAutoUpdateChanged(bool value)
    {
        WeatherPlugin.Config.AutoUpdate = value;
        WeatherPlugin.SaveConfig();
    }

    partial void OnTimeChanged(int? value)
    {
        if (value is not int value1)
        {
            return;
        }

        WeatherPlugin.Config.UpdateTime = value1;
        WeatherPlugin.SaveConfig();
    }
}
