using System.Collections.ObjectModel;
using Avalonia.Media;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.PGLauncherPlugin.ColorMC;

public partial class PGColorMCInstanceSettingModel : ObservableObject
{
    public ObservableCollection<string> Groups { get; init; } = [];

    public string[] DisplayNames { get; init; } =
    [
        LangApi.GetLang("PGColorMCInstanceSetting.Text7"),
        LangApi.GetLang("PGColorMCInstanceSetting.Text8"),
        LangApi.GetLang("PGColorMCInstanceSetting.Text9"),
    ];

    public bool HaveColorMC { get; set; }

    [ObservableProperty]
    private int _width;
    [ObservableProperty]
    private int _height;

    [ObservableProperty]
    private string _group;
    [ObservableProperty]
    private DisplayType _display;
    [ObservableProperty]
    private Color _backColor;
    [ObservableProperty]
    private Color _textColor;

    private readonly InstanceDataObj _obj;
    private readonly PGColorMCInstanceObj _config;

    private bool _load;

    public PGColorMCInstanceSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = PGColorMCPlugin.GetConfig(obj);

        if (!string.IsNullOrWhiteSpace(PGColorMCPlugin.Config.ColorMC))
        {
            HaveColorMC = File.Exists(PGColorMCPlugin.Config.ColorMC);
        }
        if (!HaveColorMC)
        {
            return;
        }

        _display = _config.Display;
        _width = _config.Width;
        _height = _config.Height;

        if (!Color.TryParse(_config.BackColor, out _backColor))
        {
            _backColor = Colors.Black;
        }
        if (!Color.TryParse(_config.TextColor, out _textColor))
        {
            _textColor = Colors.White;
        }

        var games = ColorMCUtils.GetGames();
        if (games == null)
        {
            HaveColorMC = false;
            return;
        }

        _load = true;

        var list = games.GroupBy(item => item.GroupName);
        Groups.Add("");
        foreach (var item in list)
        {
            if (string.IsNullOrWhiteSpace(item.Key))
            {
                continue;
            }
            Groups.Add(item.Key);
        }

        _group = _config.GroupName ?? "";

        _load = false;
    }

    partial void OnWidthChanged(int value)
    {
        _config.Width = value;
        PGColorMCPlugin.SaveConfig(_obj, _config);
    }

    partial void OnHeightChanged(int value)
    {
        _config.Height = value;
        PGColorMCPlugin.SaveConfig(_obj, _config);
    }

    partial void OnTextColorChanged(Color value)
    {
        _config.TextColor = value.ToString();
        PGColorMCPlugin.SaveConfig(_obj, _config);
    }

    partial void OnBackColorChanged(Color value)
    {
        _config.BackColor = value.ToString();
        PGColorMCPlugin.SaveConfig(_obj, _config);
    }

    partial void OnGroupChanged(string value)
    {
        if (_load)
        {
            return;
        }

        _config.GroupName = value;
        PGColorMCPlugin.SaveConfig(_obj, _config);
    }
}
