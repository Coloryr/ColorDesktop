using Avalonia.Media;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.AnalogClockPlugin;

public partial class AnalogClockSettingModel : ObservableObject
{
    [ObservableProperty]
    private ClockType _clockType;

    [ObservableProperty]
    private int _clockSize;

    [ObservableProperty]
    private Color _color;

    [ObservableProperty]
    private bool _blink;
    [ObservableProperty]
    private bool _display;

    private readonly AnalogClockConfigObj _config;
    private readonly InstanceDataObj _obj;

    public AnalogClockSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = AnalogClockPlugin.GetConfig(obj);

        _clockSize = _config.Size;
        _clockType = _config.Type;
        _blink = _config.Blink;
        _display = _config.DisplaySecond;

        if (!Color.TryParse(_config.Color, out _color))
        {
            _color = Colors.Red;
        }
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
