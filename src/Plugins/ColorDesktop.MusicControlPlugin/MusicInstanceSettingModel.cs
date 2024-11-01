using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.MusicControlPlugin;

public partial class MusicInstanceSettingModel : ObservableObject
{
    public string[] SkinTypes { get; init; } =
    [
        LangApi.GetLang("MusicInstanceSetting.Text4")
    ];

    [ObservableProperty]
    private int _width;
    [ObservableProperty]
    private SkinType _skin;

    private readonly InstanceDataObj _obj;
    private readonly MusicInstanceObj _config;

    public MusicInstanceSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = MusicControlPlugin.GetConfig(obj);

        _width = _config.Width;
        _skin = _config.Skin;
    }
}
