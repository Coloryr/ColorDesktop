using System.Collections.ObjectModel;
using Avalonia.Media;
using ColorDesktop.Api;
using ColorDesktop.CoreLib.View.Model;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.AnalogClockPlugin;

public partial class AnalogClockSettingModel : ObservableObject
{
    public ObservableCollection<FontDisplayModel> FontList { get; init; } = [];

    [ObservableProperty]
    private FontDisplayModel? _fontItem;

    [ObservableProperty]
    private ClockType _clockType;

    [ObservableProperty]
    private int _clockSize;
    [ObservableProperty]
    private int _textSize;

    [ObservableProperty]
    private Color _color;
    [ObservableProperty]
    private Color _textColor;
    [ObservableProperty]
    private Color _backColor;
    [ObservableProperty]
    private Color _borderColor;

    [ObservableProperty]
    private bool _blink;
    [ObservableProperty]
    private bool _display;
    [ObservableProperty]
    private bool _useFont;

    private readonly AnalogClockInstanceConfigObj _config;
    private readonly InstanceDataObj _obj;

    public AnalogClockSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = AnalogClockPlugin.GetConfig(obj);

        _clockSize = _config.Size;
        _clockType = _config.Type;
        _blink = _config.Blink;
        _display = _config.DisplaySecond;
        _useFont = _config.UseFont;
        _textSize = _config.TextSize;

        if (!Color.TryParse(_config.Color, out _color))
        {
            _color = Colors.Red;
        }
        if (!Color.TryParse(_config.TextColor, out _textColor))
        {
            _textColor = Colors.White;
        }
        if (!Color.TryParse(_config.BackColor, out _backColor))
        {
            _backColor = Colors.Cyan;
        }
        if (!Color.TryParse(_config.BorderColor, out _borderColor))
        {
            _borderColor = Colors.Cyan;
        }

        foreach (var item in FontManager.Current.SystemFonts)
        {
            FontList.Add(new()
            {
                FontName = item.Name,
                FontFamily = item
            });
        }

        _fontItem = FontList.FirstOrDefault(item => item.FontName == _config.Font);
    }

    partial void OnBorderColorChanged(Color value)
    {
        _config.BorderColor = value.ToString();
        AnalogClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBackColorChanged(Color value)
    {
        _config.BackColor = value.ToString();
        AnalogClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnTextColorChanged(Color value)
    {
        _config.TextColor = value.ToString();
        AnalogClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnTextSizeChanged(int value)
    {
        _config.TextSize = value;
        AnalogClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnUseFontChanged(bool value)
    {
        _config.UseFont = value;
        AnalogClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnFontItemChanged(FontDisplayModel? value)
    {
        if (value == null)
        {
            return;
        }
        _config.Font = value.FontName;
        AnalogClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnDisplayChanged(bool value)
    {
        _config.DisplaySecond = value;
        AnalogClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnColorChanged(Color value)
    {
        _config.Color = value.ToString();
        AnalogClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBlinkChanged(bool value)
    {
        _config.Blink = value;
        AnalogClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnClockSizeChanged(int value)
    {
        _config.Size = value;
        AnalogClockPlugin.SaveConfig(_obj, _config);
    }

    partial void OnClockTypeChanged(ClockType value)
    {
        _config.Type = value;
        AnalogClockPlugin.SaveConfig(_obj, _config);
    }
}
