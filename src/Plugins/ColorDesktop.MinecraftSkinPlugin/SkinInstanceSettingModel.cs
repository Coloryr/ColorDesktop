using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.CoreLib;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftSkinRender;

namespace ColorDesktop.MinecraftSkinPlugin;

public partial class SkinInstanceSettingModel : ObservableObject
{
    public string[] FileTypeName { get; init; } = 
    [
        LangApi.GetLang("SkinInstanceSetting.Text9"),
        LangApi.GetLang("SkinInstanceSetting.Text10"),
        LangApi.GetLang("SkinInstanceSetting.Text11")
    ];

    public string[] SkinTypeName { get; init; } =
    [
        LangApi.GetLang("SkinInstanceSetting.Text19"),
        LangApi.GetLang("SkinInstanceSetting.Text20"),
        LangApi.GetLang("SkinInstanceSetting.Text21")
    ];

    [ObservableProperty]
    private int _width;
    [ObservableProperty]
    private int _height;

    [ObservableProperty]
    private bool _lowFps;
    [ObservableProperty]
    private bool _displayFps;
    [ObservableProperty]
    private bool _enableMsaa;

    [ObservableProperty]
    private FileType _type;
    [ObservableProperty]
    private string _data;
    [ObservableProperty]
    private string _data1;
    [ObservableProperty]
    private SkinType _skin;
    [ObservableProperty]
    private bool _enableTop;
    [ObservableProperty]
    private bool _enableCape;

    [ObservableProperty]
    private bool _enableAnimation;

    [ObservableProperty]
    private bool _isFile;
    [ObservableProperty]
    private bool _isUrl;
    [ObservableProperty]
    private bool _isName;

    private readonly InstanceDataObj _obj;
    private readonly SkinInstanceObj _config;

    public SkinInstanceSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = MinecraftSkinPlugin.GetConfig(obj);

        _width = _config.Width;
        _height = _config.Height;
        _type = _config.FileType;
        _data = _config.File;
        _data1 = _config.File1;
        _lowFps = _config.LowFps;
        _displayFps = _config.DisplayFps;
        _enableMsaa = _config.EnableMSAA;
        _skin = _config.SkinType;
        _enableTop = _config.EnableTop;
        _enableCape = _config.EnableCape;
        _enableAnimation = _config.EnableAnimation;

        _isFile = _type == FileType.LocalFile;
        _isUrl = _type == FileType.Url;
        _isName = _type == FileType.Name;
    }

    partial void OnData1Changed(string value)
    {
        _config.File1 = value;
        MinecraftSkinPlugin.SaveConfig(_obj, _config);
    }

    partial void OnTypeChanged(FileType value)
    {
        IsFile = value == FileType.LocalFile;
        IsUrl = value == FileType.Url;
        IsName = value == FileType.Name;

        _config.FileType = value;
        MinecraftSkinPlugin.SaveConfig(_obj, _config);
    }

    partial void OnEnableAnimationChanged(bool value)
    {
        _config.EnableAnimation = value;
        MinecraftSkinPlugin.SaveConfig(_obj, _config);
    }

    partial void OnSkinChanged(SkinType value)
    {
        _config.SkinType = value;
        MinecraftSkinPlugin.SaveConfig(_obj, _config);
    }

    partial void OnEnableTopChanged(bool value)
    {
        _config.EnableTop = value;
        MinecraftSkinPlugin.SaveConfig(_obj, _config);
    }

    partial void OnEnableCapeChanged(bool value)
    {
        _config.EnableCape = value;
        MinecraftSkinPlugin.SaveConfig(_obj, _config);
    }

    partial void OnEnableMsaaChanged(bool value)
    {
        _config.EnableMSAA = value;
        MinecraftSkinPlugin.SaveConfig(_obj, _config);
    }

    partial void OnLowFpsChanged(bool value)
    {
        _config.LowFps = value;
        MinecraftSkinPlugin.SaveConfig(_obj, _config);
    }

    partial void OnDisplayFpsChanged(bool value)
    {
        _config.DisplayFps = value;
        MinecraftSkinPlugin.SaveConfig(_obj, _config);
    }

    partial void OnHeightChanged(int value)
    {
        _config.Height = value;
        MinecraftSkinPlugin.SaveConfig(_obj, _config);
    }

    partial void OnWidthChanged(int value)
    {
        _config.Width = value;
        MinecraftSkinPlugin.SaveConfig(_obj, _config);
    }

    [RelayCommand]
    public async Task SelectFile(Control? control)
    {
        var top = TopLevel.GetTopLevel(control);
        if (top == null)
        {
            return;
        }
        var res = await CoreHelper.SelectFile(top, LangApi.GetLang("SkinInstanceSetting.Info1"),
            ["*.png"], LangApi.GetLang("SkinInstanceSetting.Info2"));

        if (res == null || !res.Any())
        {
            return;
        }

        Data = res[0].GetPath()!;
    }

    [RelayCommand]
    public async Task SelectFile1(Control? control)
    {
        var top = TopLevel.GetTopLevel(control);
        if (top == null)
        {
            return;
        }
        var res = await CoreHelper.SelectFile(top, LangApi.GetLang("SkinInstanceSetting.Info3"),
            ["*.png"], LangApi.GetLang("SkinInstanceSetting.Info4"));

        if (res == null || !res.Any())
        {
            return;
        }

        Data1 = res[0].GetPath()!;
    }
}
