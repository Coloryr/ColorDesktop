﻿using System.Collections.ObjectModel;
using System.Globalization;
using Avalonia.Media;
using ColorDesktop.WeatherPlugin.Objs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.WeatherPlugin;

public partial class WeatherModel : ObservableObject
{
    public static readonly Dictionary<string, string> WindIcon = new()
    {
        { "东北风", "/Resource/Wind/north_east.svg" },
        { "东风", "/Resource/Wind/east.svg" },
        { "东南风", "/Resource/Wind/south_east.svg" },
        { "南风", "/Resource/Wind/south.svg" },
        { "西南风", "/Resource/Wind/south_west.svg" },
        { "西风", "/Resource/Wind/west.svg" },
        { "西北风", "/Resource/Wind/north_west.svg" },
        { "北风", "/Resource/Wind/north.svg" }
    };

    public static readonly Dictionary<string, string> WeaterIcon = new()
    {
        { "晴", "/Resource/Weather/sun.svg" },
        { "晴间多云", "/Resource/Weather/sun_cloud.svg" },
        { "多云", "/Resource/Weather/cloud_lot.svg" },
        { "阴", "/Resource/Weather/cloud.svg" },
        { "阵雨", "/Resource/Weather/rain.svg" },
        { "雷阵雨", "/Resource/Weather/lightning_rain.svg" },
        { "雷阵雨并伴有冰雹", "/Resource/Weather/lightning_rain_hail.svg" },
        { "雨夹雪", "/Resource/Weather/rain_snow.svg" },
        { "小雨", "/Resource/Weather/rain.svg" },
        { "中雨", "/Resource/Weather/rain_lot.svg" },
        { "大雨", "/Resource/Weather/rain_lot_lot.svg" },
        { "暴雨", "/Resource/Weather/rain_lot_lot.svg" },
        { "大暴雨", "/Resource/Weather/rain_lot_lot_lot.svg" },
        { "特大暴雨", "/Resource/Weather/rain_lot_lot_lot.svg" },
        { "阵雪", "/Resource/Weather/snow.svg" },
        { "小雪", "/Resource/Weather/snow.svg" },
        { "中雪", "/Resource/Weather/snow_lot.svg" },
        { "大雪", "/Resource/Weather/snow_lot_lot.svg" },
        { "暴雪", "/Resource/Weather/snow_lot_lot.svg" },
        { "雾", "/Resource/Weather/fog.svg" },
        { "冻雨", "/Resource/Weather/rain_snow_cold.svg" },
        { "沙尘暴", "/Resource/Weather/dust_lot.svg" },
        { "小到中雨", "/Resource/Weather/rain_lot.svg" },
        { "中到大雨", "/Resource/Weather/rain_lot_lot.svg" },
        { "大到暴雨", "/Resource/Weather/rain_lot_lot_lot.svg" },
        { "大暴雨到特大暴雨", "/Resource/Weather/rain_fast.svg" },
        { "小到中雪", "/Resource/Weather/snow.svg" },
        { "中到大雪", "/Resource/Weather/snow_lot.svg" },
        { "大到暴雪", "/Resource/Weather/snow_lot_lot.svg" },
        { "浮尘", "/Resource/Weather/dust.svg" },
        { "扬沙", "/Resource/Weather/dust_lot.svg" },
        { "强沙尘暴", "/Resource/Weather/dust_lot_lot.svg" },
        { "浓雾", "/Resource/Weather/fog_lot.svg" },
        { "龙卷风", "/Resource/Weather/tornado.svg" },
        { "弱高吹雪", "/Resource/Weather/snow.svg" },
        { "轻雾", "/Resource/Weather/fog_lot.svg" },
        { "强浓雾", "/Resource/Weather/fog_lot.svg" },
        { "霾", "/Resource/Weather/fog.svg" },
        { "中度霾", "/Resource/Weather/fog.svg" },
        { "重度霾", "/Resource/Weather/fog_lot.svg" },
        { "严重霾", "/Resource/Weather/fog_lot.svg" },
        { "大雾", "/Resource/Weather/fog_lot.svg" },
        { "特强浓雾", "/Resource/Weather/fog_lot.svg" },
        { "雨", "/Resource/Weather/rain.svg" },
        { "雪", "/Resource/Weather/snow.svg" },
        //{ "有风", "/Resource/Weather/wind.svg" },
        //{ "平静", "/Resource/Weather/wind.svg" },
        //{ "微风", "/Resource/Weather/wind.svg" },
        //{ "清风", "/Resource/Weather/wind.svg" },
        //{ "强风/劲风", "/Resource/Weather/wind_lot.svg" },
        //{ "疾风", "/Resource/Weather/wind_lot.svg" },
        //{ "大风", "/Resource/Weather/wind_lot.svg" },
        //{ "烈风", "/Resource/Weather/wind_lot.svg" },
        //{ "风暴", "/Resource/Weather/wind_lot_lot.svg" },
        //{ "狂爆风", "/Resource/Weather/wind_lot_lot.svg" },
        //{ "飓风", "/Resource/Weather/wind_lot_lot.svg" },
        //{ "热带风暴", "/Resource/Weather/wind_lot_lot.svg" },
        //{ "强阵雨", "/Resource/Weather/rain_fast.svg" },
        //{ "强雷阵雨", "/Resource/Weather/lightning_rain_fast.svg" },
        //{ "极端降雨", "/Resource/Weather/rain_fast.svg" },
        //{ "毛毛雨/细雨", "/Resource/Weather/rain_low.svg" },
        //{ "雨雪天气", "/Resource/Weather/rain_snow.svg" },
        //{ "阵雨夹雪", "/Resource/Weather/rain_snow.svg" },
        //{ "热", "/Resource/Weather/hot.svg" },
        //{ "冷", "/Resource/Weather/cold.svg" },
        //{ "未知", "/Resource/Weather/none.svg" },
    };

    [ObservableProperty]
    private string _img;

    [ObservableProperty]
    private string _value;
    [ObservableProperty]
    private string _city;
    [ObservableProperty]
    private string _state;
    [ObservableProperty]
    private string _province;
    [ObservableProperty]
    private string _winddirection;
    [ObservableProperty]
    private string _windpower;
    [ObservableProperty]
    private string _humidity;
    [ObservableProperty]
    private string _time;
    [ObservableProperty]
    private string _wind;
    [ObservableProperty]
    private string _timeNext;

    [ObservableProperty]
    private IBrush _backColor;
    [ObservableProperty]
    private IBrush _textColor;

    [ObservableProperty]
    private bool _error;
    [ObservableProperty]
    private bool _isUpdate;
    [ObservableProperty]
    private bool _haveWindPower;
    [ObservableProperty]
    private bool _showNextDay;
    [ObservableProperty]
    private bool _nextDayLoad;
    [ObservableProperty]
    private bool _nextDayLoadError;
    [ObservableProperty]
    private bool _showButton;

    private DateTime _date;

    public ObservableCollection<WeatherDayModel> WeatherDays { get; init; } = [];

    private City1Obj? _obj;

    [RelayCommand]
    public async Task NextDay()
    {
        if (_obj == null)
        {
            NextDayLoadError = true;
            NextDayLoad = false;
            return;
        }

        if (ShowNextDay)
        {
            ShowNextDay = false;
        }
        else
        {
            ShowNextDay = true;

            NextDayLoad = true;

            var data = await ColorMCApi.GetData(_obj.Adcode);

            if (data?.Result.Forecasts is { } res)
            {
                NextDayLoadError = false;
                WeatherDays.Clear();

                foreach (var item in res)
                {
                    var obj = new WeatherDayModel(item)
                    {
                        TextColor = TextColor
                    };
                    WeatherDays.Add(obj);
                }

                var time = DateTime.ParseExact(data.Result.Now.Uptime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                TimeNext = time.ToString("G");
            }
            else
            {
                NextDayLoadError = true;
            }

            NextDayLoad = false;
        }
    }

    [RelayCommand]
    public async Task Update()
    {
        if (_obj == null)
        {
            Error = true;
            return;
        }

        IsUpdate = true;
        var data = await ColorMCApi.GetData(_obj.Adcode);
        if (data?.Result is { } res)
        {
            Error = false;
            Value = res.Now.Temp.ToString();
            State = res.Now.Text;
            if (res.Location.Province == res.Location.Name)
            {
                Province = res.Location.Province;
                City = res.Location.Name;
            }
            else
            {
                Province = res.Location.City;
                City = res.Location.Name;
            }
            Winddirection = res.Now.WindDir;
            Windpower = res.Now.WindClass;
            Humidity = res.Now.Rh.ToString();
            var time = DateTime.ParseExact(res.Now.Uptime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

            Time = time.ToString("G");

            if (Windpower == "无风向" || Windpower == "旋转不定")
            {
                HaveWindPower = false;
            }
            else
            {
                HaveWindPower = true;
            }

            if (WindIcon.TryGetValue(Winddirection, out var wind))
            {
                Wind = wind;
            }
            else
            {
                Wind = "/Resource/Weather/none.svg";
            }

            if (WeaterIcon.TryGetValue(State, out var weather))
            {
                Img = weather;
            }
            else
            {
                Img = "/Resource/Weather/none.svg";
            }
        }
        else
        {
            Error = true;
        }
        IsUpdate = false;
    }

    public async void Tick()
    {
        if (WeatherPlugin.Config.AutoUpdate == false)
        {
            return;
        }

        var now = DateTime.Now;
        var less = now - _date;
        if (less.TotalMinutes >= WeatherPlugin.Config.UpdateTime)
        {
            _date = now;
            await Update();
        }
    }

    public void Update(WeatherInstanceObj obj)
    {
        _date = DateTime.Now;
        try
        {
            BackColor = Brush.Parse(obj.BackColor);
            TextColor = Brush.Parse(obj.TextColor);
        }
        catch
        { 
            
        }
        _obj = ColorMCApi.GetCityAdcode(int.Parse(obj.City));
        _ = Update();
        if (ShowNextDay)
        {
            _ = NextDay();
        }
    }
}
