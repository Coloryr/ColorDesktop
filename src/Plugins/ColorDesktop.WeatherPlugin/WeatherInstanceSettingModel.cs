using System.Collections.ObjectModel;
using Avalonia.Media;
using AvaloniaEdit.Utils;
using ColorDesktop.Api.Objs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.WeatherPlugin;

public partial class WeatherInstanceSettingModel : ObservableObject
{
    private static readonly Color s_color1 = Color.Parse("#0294FF");

    [ObservableProperty]
    private Color _backColor;
    [ObservableProperty]
    private Color _textColor;

    [ObservableProperty]
    private int _cityIndex1 = -1;
    [ObservableProperty]
    private int _cityIndex2 = -1;
    [ObservableProperty]
    private int _cityIndex3 = -1;

    [ObservableProperty]
    private bool _cityDisplay1;
    [ObservableProperty]
    private bool _cityDisplay2;

    public string[] City { get; init; } = AmapApi.GetCityName();
    public ObservableCollection<string> City1 { get; init; } = [];
    public ObservableCollection<string> City2 { get; init; } = [];

    private readonly WeatherInstanceObj _config;
    private readonly InstanceDataObj _obj;

    private bool _isLoad;

    public WeatherInstanceSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = WeatherPlugin.GetConfig(obj);

        if (!Color.TryParse(_config.BackColor, out _backColor))
        {
            _backColor = s_color1;
        }
        if (!Color.TryParse(_config.TextColor, out _textColor))
        {
            _textColor = Colors.White;
        }

        var indexs = AmapApi.GetCityIndexAdcode(int.Parse(_config.City));

        _isLoad = true;

        CityIndex1 = indexs.Item1;
        CityIndex2 = indexs.Item2;
        CityIndex3 = indexs.Item3;

        _isLoad = false;
    }

    partial void OnCityIndex1Changed(int value)
    {
        var city = AmapApi.GetCityIndex(value);
        if (city != null)
        {
            City1.Clear();
            var list = AmapApi.GetCityName(value);
            if (list.Count != 0)
            {
                CityDisplay1 = true;
                City1.AddRange(list);
                if (!_isLoad)
                {
                    CityIndex2 = 0;
                }
            }
            else
            {
                CityDisplay1 = false;
                CityIndex2 = -1;
                CityIndex3 = -1;
                return;
            }
            if (!_isLoad)
            {
                _config.City = city.Adcode.ToString();
                WeatherPlugin.SaveConfig(_obj, _config);
            }
        }
    }
    partial void OnCityIndex2Changed(int value)
    {
        if (value == -1)
        {
            City2.Clear();
            return;
        }
        var city = AmapApi.GetCityIndex(CityIndex1, value);
        if (city != null)
        {
            City2.Clear();
            var list = AmapApi.GetCityName(CityIndex1, value);
            if (list.Count != 0)
            {
                CityDisplay2 = true;
                City2.AddRange(list);
                if (!_isLoad)
                {
                    CityIndex3 = -1;
                    CityIndex3 = 0;
                }
            }
            else
            {
                CityDisplay2 = false;
                CityIndex3 = -1;
                return;
            }
            if (!_isLoad)
            {
                _config.City = city.Adcode.ToString();
                WeatherPlugin.SaveConfig(_obj, _config);
            }
        }
    }

    partial void OnCityIndex3Changed(int value)
    {
        if (value == -1 || _isLoad)
        {
            return;
        }
        var city = AmapApi.GetCityIndex(CityIndex1, CityIndex2, value);
        if (city != null)
        {
            _config.City = city.Adcode.ToString();
            WeatherPlugin.SaveConfig(_obj, _config);
        }
    }

    partial void OnTextColorChanged(Color value)
    {
        _config.TextColor = value.ToString();
        WeatherPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBackColorChanged(Color value)
    {
        _config.BackColor = value.ToString();
        WeatherPlugin.SaveConfig(_obj, _config);
    }
}