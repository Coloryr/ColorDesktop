using Avalonia.Media;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.CalendarPlugin;

public partial class CalendarInstanceSettingModel : ObservableObject
{
    public string[] WeekName { get; init; } =
    [
        LangApi.GetLang("CalendarSKin2.Text1"),
        LangApi.GetLang("CalendarSKin2.Text7"),
        LangApi.GetLang("CalendarSKin2.Text2")
    ];
    public string[] SkinName { get; init; } =
    [
        LangApi.GetLang("CalendarInstanceSetting.Text6"),
        LangApi.GetLang("CalendarInstanceSetting.Text7"),
        LangApi.GetLang("CalendarInstanceSetting.Text8"),
    ];

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
