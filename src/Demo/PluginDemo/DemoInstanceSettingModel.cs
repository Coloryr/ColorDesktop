using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Api.Objs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PluginDemo;

public partial class DemoInstanceSettingModel : ObservableObject
{
    private readonly DemoObj _config;
    private readonly InstanceDataObj _obj;

    [ObservableProperty]
    private string _text;

    public DemoInstanceSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = DemoPlugin.GetConfig(obj);

        _text = _config.Text;
    }

    partial void OnTextChanged(string value)
    {
        _config.Text = value;
        DemoPlugin.SaveConfig(_obj, _config);
    }
}
