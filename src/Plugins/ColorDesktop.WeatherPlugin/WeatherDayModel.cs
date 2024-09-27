using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.WeatherPlugin.Objs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.WeatherPlugin;

public partial class WeatherDayModel : ObservableObject
{
    private static readonly Dictionary<string, string> WeekTran = new()
    {
        { "1", "一" },
        { "2", "二" },
        { "3", "三" },
        { "4", "四" },
        { "5", "五" },
        { "6", "六" },
        { "7", "日" },
    };

    public string Date => _obj.Date;
    public string Week { get; init; }
    public string DayWeather => _obj.Dayweather;
    public string DayIcon { get; init; }
    public string DayTemp => _obj.Daytemp;

    public string NightWeather => _obj.Nightweather;
    public string NightIcon { get; init; }
    public string NightTemp => _obj.Nighttemp;

    private readonly WeatherInfoObj.ForecastObj.CastObj _obj;

    public WeatherDayModel(WeatherInfoObj.ForecastObj.CastObj obj)
    {
        _obj = obj;
        if (WeekTran.TryGetValue(obj.Week, out var week))
        {
            Week = week;
        }
        if (WeatherModel.WeaterIcon.TryGetValue(obj.Dayweather, out var icon))
        {
            DayIcon = icon;
        }
        else
        {
            DayIcon = "";
        }
        if (WeatherModel.WeaterIcon.TryGetValue(obj.Nightweather, out var icon1))
        {
            NightIcon = icon1;
        }
        else
        {
            NightIcon = "";
        }
    }
}
