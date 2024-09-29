using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.CalendarPlugin;

public partial class CalendarInstanceSettingModel : ObservableObject
{
    [ObservableProperty]
    private Color _backColor;

    [ObservableProperty]
    private Color _textColor;

    private readonly CalendarInstanceObj _config;
    private readonly InstanceDataObj _obj;

    public CalendarInstanceSettingModel(InstanceDataObj obj)
    {
        _config = CalendarPlugin.GetConfig(obj);
        _obj = obj;

        if (!Color.TryParse(_config.BackColor, out _backColor))
        {
            _backColor = Colors.Black;
        }

        if (!Color.TryParse(_config.TextColor, out _textColor))
        {
            _textColor = Colors.White;
        }
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
