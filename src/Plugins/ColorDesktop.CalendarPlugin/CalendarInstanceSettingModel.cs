using Avalonia.Media;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.CalendarPlugin;

public partial class CalendarInstanceSettingModel : ObservableObject
{
    public string[] WeekName { get; init; } = ["星期日", "星期六", "星期一"];
    public string[] SkinName { get; init; } = ["初始皮肤", "皮肤1"];

    [ObservableProperty]
    private Color _backColor;
    [ObservableProperty]
    private Color _textColor;

    [ObservableProperty]
    private WeekStart _weekStart;

    [ObservableProperty]
    private SkinType _skin;

    private readonly CalendarInstanceObj _config;
    private readonly InstanceDataObj _obj;

    public CalendarInstanceSettingModel(InstanceDataObj obj)
    {
        _config = CalendarPlugin.GetConfig(obj);
        _obj = obj;

        _skin = _config.Skin;

        if (!Color.TryParse(_config.BackColor, out _backColor))
        {
            _backColor = Colors.Black;
        }

        if (!Color.TryParse(_config.TextColor, out _textColor))
        {
            _textColor = Colors.White;
        }

        _weekStart = _config.WeekStart;
    }

    partial void OnSkinChanged(SkinType value)
    {
        _config.Skin = value;
        CalendarPlugin.SaveConfig(_obj, _config);
    }

    partial void OnWeekStartChanged(WeekStart value)
    {
        _config.WeekStart = value;
        CalendarPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBackColorChanged(Color value)
    {
        _config.BackColor = value.ToString();
        CalendarPlugin.SaveConfig(_obj, _config);
    }

    partial void OnTextColorChanged(Color value)
    {
        _config.TextColor = value.ToString();
        CalendarPlugin.SaveConfig(_obj, _config);
    }
}
