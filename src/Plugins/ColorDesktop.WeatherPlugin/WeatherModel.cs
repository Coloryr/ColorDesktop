﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ColorDesktop.WeatherPlugin.Objs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.WeatherPlugin;

public partial class WeatherModel : ObservableObject
{
    private static readonly Dictionary<string, string> WindIcon = new()
    {
        { "东北", "/Resource/Wind/north_east.svg" },
        { "东", "/Resource/Wind/east.svg" },
        { "东南", "/Resource/Wind/south_east.svg" },
        { "南", "/Resource/Wind/south.svg" },
        { "西南", "/Resource/Wind/south_west.svg" },
        { "西", "/Resource/Wind/west.svg" },
        { "西北", "/Resource/Wind/north_west.svg" },
        { "北", "/Resource/Wind/north.svg" }
    };

    private static readonly Dictionary<string, string> WeaterIcon = new()
    {
        { "晴", "/Resource/Weather/sun.svg" },
        { "少云", "/Resource/Weather/cloud.svg" },
        { "晴间多云", "/Resource/Weather/sun_cloud.svg" },
        { "多云", "/Resource/Weather/cloud_log.svg" },
        { "阴", "/Resource/Weather/cloud.svg" },
        { "有风", "/Resource/Weather/wind.svg" },
        { "平静", "/Resource/Weather/wind.svg" },
        { "微风", "/Resource/Weather/wind.svg" },
        { "清风", "/Resource/Weather/wind.svg" },
        { "强风/劲风", "/Resource/Weather/wind_lot.svg" },
        { "疾风", "/Resource/Weather/wind_lot.svg" },
        { "大风", "/Resource/Weather/wind_lot.svg" },
        { "烈风", "/Resource/Weather/wind_lot.svg" },
        { "风暴", "/Resource/Weather/wind_lot_lot.svg" },
        { "狂爆风", "/Resource/Weather/wind_lot_lot.svg" },
        { "飓风", "/Resource/Weather/wind_lot_lot.svg" },
        { "热带风暴", "/Resource/Weather/wind_lot_lot.svg" },
        { "霾", "/Resource/Weather/fog.svg" },
        { "中度霾", "/Resource/Weather/fog.svg" },
        { "重度霾", "/Resource/Weather/fog_lot.svg" },
        { "严重霾", "/Resource/Weather/fog_lot.svg" },
        { "阵雨", "/Resource/Weather/rain.svg" },
        { "雷阵雨", "/Resource/Weather/lightning_rain.svg" },
        { "雷阵雨并伴有冰雹", "/Resource/Weather/lightning_rain_hail.svg" },
        { "小雨", "/Resource/Weather/rain.svg" },
        { "中雨", "/Resource/Weather/rain_lot.svg" },
        { "大雨", "/Resource/Weather/rain_lot_lot.svg" },
        { "暴雨", "/Resource/Weather/rain_lot_lot.svg" },
        { "大暴雨", "/Resource/Weather/rain_lot_lot_lot.svg" },
        { "特大暴雨", "/Resource/Weather/rain_lot_lot_lot.svg" },
        { "强阵雨", "/Resource/Weather/rain_fast.svg" },
        { "强雷阵雨", "/Resource/Weather/lightning_rain_fast.svg" },
        { "极端降雨", "/Resource/Weather/rain_fast.svg" },
        { "毛毛雨/细雨", "/Resource/Weather/rain_low.svg" },
        { "雨", "/Resource/Weather/rain.svg" },
        { "小雨-中雨", "/Resource/Weather/rain_lot.svg" },
        { "中雨-大雨", "/Resource/Weather/rain_lot_lot.svg" },
        { "大雨-暴雨", "/Resource/Weather/rain_lot_lot_lot.svg" },
        { "大暴雨-特大暴雨", "/Resource/Weather/rain_fast.svg" },
        { "雨雪天气", "/Resource/Weather/rain_snow.svg" },
        { "雨夹雪", "/Resource/Weather/rain_snow.svg" },
        { "阵雨夹雪", "/Resource/Weather/rain_snow.svg" },
        { "冻雨", "/Resource/Weather/rain_snow_cold.svg" },
        { "雪", "/Resource/Weather/snow.svg" },
        { "阵雪", "/Resource/Weather/snow.svg" },
        { "小雪", "/Resource/Weather/snow.svg" },
        { "中雪", "/Resource/Weather/snow_lot.svg" },
        { "大雪", "/Resource/Weather/snow_lot_lot.svg" },
        { "暴雪", "/Resource/Weather/snow_lot_lot.svg" },
        { "小雪-中雪", "/Resource/Weather/snow.svg" },
        { "中雪-大雪", "/Resource/Weather/snow_lot.svg" },
        { "大雪-暴雪", "/Resource/Weather/snow_lot_lot.svg" },
        { "浮尘", "/Resource/Weather/dust.svg" },
        { "扬沙", "/Resource/Weather/dust_lot.svg" },
        { "沙尘暴", "/Resource/Weather/dust_lot.svg" },
        { "强沙尘暴", "/Resource/Weather/dust_lot_lot.svg" },
        { "龙卷风", "/Resource/Weather/tornado.svg" },
        { "雾", "/Resource/Weather/fog.svg" },
        { "浓雾", "/Resource/Weather/fog_lot.svg" },
        { "强浓雾", "/Resource/Weather/fog_lot.svg" },
        { "轻雾", "/Resource/Weather/fog_lot.svg" },
        { "大雾", "/Resource/Weather/fog_lot.svg" },
        { "特强浓雾", "/Resource/Weather/fog_lot.svg" },
        { "热", "/Resource/Weather/hot.svg" },
        { "冷", "/Resource/Weather/cold.svg" },
        { "未知", "/Resource/Weather/none.svg" },
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
    private IBrush _backColor;
    [ObservableProperty]
    private IBrush _textColor;

    [ObservableProperty]
    private bool _error;
    [ObservableProperty]
    private bool _isUpdate;
    [ObservableProperty]
    private bool _haveWindPower;

    private City1Obj? _obj;

    [RelayCommand]
    public async Task Update()
    {
        if (_obj == null)
        {
            Error = true;
            return;
        }
        
        IsUpdate = true;
        var data = await AmapApi.GetData(_obj.Adcode, false);
        if (data?.Lives.FirstOrDefault() is { } res)
        {
            Error = false;
            Value = res.TemperatureFloat;
            City = res.City;
            State = res.Weather;
            Province = res.Province;
            Winddirection = res.Winddirection;
            Windpower = res.Windpower;
            Humidity = res.HumidityFloat;
            Time = res.Reporttime.ToString("U");

            if (res.Windpower == "无风向" || res.Windpower == "旋转不定")
            {
                HaveWindPower = false;
            }
            else
            {
                HaveWindPower = true;
            }

            if (WindIcon.TryGetValue(res.Winddirection, out var wind))
            {
                Wind = wind;
            }
            else
            {
                Wind = "";
            }

            if (WeaterIcon.TryGetValue(res.Weather, out var weather))
            {
                Img = weather;
            }
            else
            {
                Img = "";
            }
        }
        else
        {
            Error = true;
        }
        IsUpdate = false;
    }

    public void Tick()
    {
        //if (_test)
        //{
        //    return;
        //}
        //_test = true;

        //await Update();
    }

    public async void Update(WeatherInstanceObj obj)
    {
        BackColor = Brush.Parse(obj.BackColor);
        TextColor = Brush.Parse(obj.TextColor);
        _obj = AmapApi.GetCityAdcode(int.Parse(obj.City));
        await Update();
    }
}
