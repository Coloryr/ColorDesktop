using System.Collections.ObjectModel;
using Avalonia.Media;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.ClockPlugin;

public partial class ClockInstanceSettingModel : ObservableObject
{
    public ObservableCollection<FontDisplayModel> FontList { get; init; } = [];

    [ObservableProperty]
    private FontDisplayModel? _fontItem;

    [ObservableProperty]
    private bool _useFont;
    [ObservableProperty]
    private bool _displaySecond;
    [ObservableProperty]
    private bool _blink;
    [ObservableProperty]
    private bool _customColor;
    [ObservableProperty]
    private bool _customSize;

    [ObservableProperty]
    private Color _color;
    [ObservableProperty]
    private int _size;

    [ObservableProperty]
    private Color _hourColor;
    [ObservableProperty]
    private Color _minuteColor;
    [ObservableProperty]
    private Color _secondColor;
    [ObservableProperty]
    private Color _colonColor;

    [ObservableProperty]
    private int _hourSize;
    [ObservableProperty]
    private int _minuteSize;
    [ObservableProperty]
    private int _secondSize;
    [ObservableProperty]
    private int _colonSize;

    [ObservableProperty]
    private Color _backGround;

    private readonly ClockInstanceObj _config;
    private readonly InstanceDataObj _obj;

    public ClockInstanceSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = ClockPlugin.GetConfig(obj);

        foreach (var item in FontManager.Current.SystemFonts)
        {
            FontList.Add(new()
            {
                FontName = item.Name,
                FontFamily = item
            });
        }

        _fontItem = FontList.FirstOrDefault(item => item.FontName == _config.FontName);

        _useFont = _config.UseFont;
        _displaySecond = _config.DisplaySecond;
        _blink = _config.Blink;
        _customColor = _config.CustomColor;
        _customSize = _config.CustomSize;
        _size = _config.Size;
        _hourSize = _config.HourSize;
        _minuteSize = _config.MinuteSize;
        _secondSize = _config.SecondSize;
        _colonSize = _config.ColonSize;

        if (Color.TryParse(_config.Color, out var color))
        {
            _color = color;
        }
        else
        {
            _color = Colors.Black;
        }

        if (Color.TryParse(_config.HourColor, out color))
        {
            _hourColor = color;
        }
        else
        {
            _hourColor = Colors.Black;
        }
        if (Color.TryParse(_config.MinuteColor, out color))
        {
            _minuteColor = color;
        }
        else
        {
            _minuteColor = Colors.Black;
        }
        if (Color.TryParse(_config.SecondColor, out color))
        {
            _secondColor = color;
        }
        else
        {
            _secondColor = Colors.Black;
        }
        if (Color.TryParse(_config.ColonColor, out color))
        {
            _colonColor = color;
        }
        else
        {
            _colonColor = Colors.Black;
        }

        if (Color.TryParse(_config.BackGround, out color))
        {
            _backGround = color;
        }
        else
        {
            _backGround = Colors.Transparent;
        }
    }

    partial void OnUseFontChanged(bool value)
    {
        _config.UseFont = value;
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnFontItemChanged(FontDisplayModel? value)
    {
        if (value == null)
        {
            return;
        }
        _config.FontName = value.FontName;
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnDisplaySecondChanged(bool value)
    {
        _config.DisplaySecond = value;
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBlinkChanged(bool value)
    {
        _config.Blink = value;
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnCustomColorChanged(bool value)
    {
        _config.CustomColor = value;
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnCustomSizeChanged(bool value)
    {
        _config.CustomSize = value;
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnColorChanged(Color value)
    {
        _config.Color = value.ToString();
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnSizeChanged(int value)
    {
        _config.Size = value;
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnHourColorChanged(Color value)
    {
        _config.HourColor = value.ToString();
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnHourSizeChanged(int value)
    {
        _config.HourSize = value;
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnMinuteColorChanged(Color value)
    {
        _config.MinuteColor = value.ToString();
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnMinuteSizeChanged(int value)
    {
        _config.MinuteSize = value;
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnSecondColorChanged(Color value)
    {
        _config.SecondColor = value.ToString();
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnSecondSizeChanged(int value)
    {
        _config.SecondSize = value;
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBackGroundChanged(Color value)
    {
        _config.BackGround = value.ToString();
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnColonSizeChanged(int value)
    {
        _config.ColonSize = value;
        ClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnColonColorChanged(Color value)
    {
        _config.ColonColor = value.ToString();
        ClockPlugin.SaveConfig(_obj, _config);
    }
}
