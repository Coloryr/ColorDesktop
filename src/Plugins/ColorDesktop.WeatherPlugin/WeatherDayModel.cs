using Avalonia.Media;
using ColorDesktop.WeatherPlugin.Objs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.WeatherPlugin;

public partial class WeatherDayModel : ObservableObject
{
    public string Date => _obj.Date;
    public string Week { get; init; }
    public string DayWeather => _obj.TextDay;
    public string DayIcon { get; init; }
    public string DayTemp => _obj.High.ToString();

    public string NightWeather => _obj.TextNight;
    public string NightIcon { get; init; }
    public string NightTemp => _obj.Low.ToString();

    public IBrush TextColor { get; init; }

    private readonly WeatherInfoObj.ResultObj.ForecastObj _obj;

    public WeatherDayModel(WeatherInfoObj.ResultObj.ForecastObj obj)
    {
        _obj = obj;
        Week = obj.Week;
        if (WeatherModel.WeaterIcon.TryGetValue(obj.TextDay, out var icon))
        {
            DayIcon = icon;
        }
        else
        {
            DayIcon = "";
        }
        if (WeatherModel.WeaterIcon.TryGetValue(obj.TextNight, out var icon1))
        {
            NightIcon = icon1;
        }
        else
        {
            NightIcon = "";
        }
    }
}
