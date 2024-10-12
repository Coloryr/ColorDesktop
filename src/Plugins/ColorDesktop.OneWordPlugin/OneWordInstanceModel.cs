using Avalonia.Media;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.OneWordPlugin;

public partial class OneWordInstanceModel : ObservableObject
{
    [ObservableProperty]
    private int _width;
    [ObservableProperty]
    private int _size;
    [ObservableProperty]
    private Color _text;
    [ObservableProperty]
    private Color _back;

    private readonly InstanceDataObj _obj;
    private readonly OneWordInstanceObj _config;

    public OneWordInstanceModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = OneWordPlugin.GetConfig(obj);

        _width = _config.Width;
        _size = _config.Size;
        if (!Color.TryParse(_config.TextColor, out _text))
        {
            _text = Colors.White;
        }
        if (!Color.TryParse(_config.BackColor, out _back))
        {
            _back = Colors.Black;
        }
    }

    partial void OnSizeChanged(int value)
    {
        _config.Size = value;
        OneWordPlugin.SaveConfig(_obj, _config);
    }

    partial void OnWidthChanged(int value)
    {
        _config.Width = value;
        OneWordPlugin.SaveConfig(_obj, _config);
    }

    partial void OnTextChanged(Color value)
    {
        _config.TextColor = value.ToString();
        OneWordPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBackChanged(Color value)
    {
        _config.BackColor = value.ToString();
        OneWordPlugin.SaveConfig(_obj, _config);
    }
}
