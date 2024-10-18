using Avalonia.Media;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.BmPlugin;

public partial class BmInstanceSettingModel : ObservableObject
{
    public string[] SkinName { get; init; } =
    [
        LangApi.GetLang("BmInstanceSettingControl.Text2"),
        LangApi.GetLang("BmInstanceSettingControl.Text3"),
        LangApi.GetLang("BmInstanceSettingControl.Text8")
    ];

    [ObservableProperty]
    private SkinType _skin;

    [ObservableProperty]
    private int _width;
    [ObservableProperty]
    private int _height;

    [ObservableProperty]
    private int _width1;

    [ObservableProperty]
    private Color _color1;
    [ObservableProperty]
    private Color _color2;

    [ObservableProperty]
    private bool _displaySkin2;
    [ObservableProperty]
    private bool _displaySkin3;

    private readonly BmInstanceObj _config;
    private readonly InstanceDataObj _obj;

    public BmInstanceSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = BmPlugin.GetConfig(obj);
        _skin = _config.Skin;
        _width = _config.Width;
        _height = _config.Height;
        _width1 = _config.Width1;

        if (!Color.TryParse(_config.Color1, out _color1))
        {
            _color1 = Colors.Black;
        }
        if (!Color.TryParse(_config.Color2, out _color2))
        {
            _color2 = Colors.White;
        }

        LoadSetting(_config.Skin);
    }

    partial void OnWidth1Changed(int value)
    {
        _config.Width1 = value;
        BmPlugin.SaveConfig(_obj, _config);
    }

    partial void OnColor1Changed(Color value)
    {
        _config.Color1 = value.ToString();
        BmPlugin.SaveConfig(_obj, _config);
    }

    partial void OnColor2Changed(Color value)
    {
        _config.Color2 = value.ToString();
        BmPlugin.SaveConfig(_obj, _config);
    }

    partial void OnWidthChanged(int value)
    {
        _config.Width = value;
        BmPlugin.SaveConfig(_obj, _config);
    }

    partial void OnHeightChanged(int value)
    {
        _config.Height = value;
        BmPlugin.SaveConfig(_obj, _config);
    }

    partial void OnSkinChanged(SkinType value)
    {
        LoadSetting(value);
        _config.Skin = value;
        BmPlugin.SaveConfig(_obj, _config);
    }

    private void LoadSetting(SkinType value)
    {
        switch (value)
        {
            case SkinType.Skin1:
                DisplaySkin2 = false;
                DisplaySkin3 = false;
                break;
            case SkinType.Skin2:
                DisplaySkin2 = true;
                DisplaySkin3 = false;
                break;
            case SkinType.Skin3:
                DisplaySkin2 = true;
                DisplaySkin3 = true;
                break;
        }
    }
}
