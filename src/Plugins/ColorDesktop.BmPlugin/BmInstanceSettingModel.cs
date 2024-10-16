using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.BmPlugin;

public partial class BmInstanceSettingModel : ObservableObject
{
    public string[] SkinName { get; init; } = ["皮肤1", "皮肤2"];

    [ObservableProperty]
    private SkinType _skin;

    private readonly BmInstanceObj _config;
    private readonly InstanceDataObj _obj;

    public BmInstanceSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = BmPlugin.GetConfig(obj);
        _skin = _config.Skin;
    }

    partial void OnSkinChanged(SkinType value)
    {
        _config.Skin = value;
        BmPlugin.SaveConfig(_obj, _config);
    }
}
