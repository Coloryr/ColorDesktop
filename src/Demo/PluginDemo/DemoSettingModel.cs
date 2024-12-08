using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PluginDemo;

public partial class DemoSettingModel : ObservableObject
{
    [ObservableProperty]
    private string _text;

    public DemoSettingModel()
    {
        var config = DemoPlugin.Config;
        if (config == null)
        {
            return;
        }

        Text = config.Text;
    }

    partial void OnTextChanged(string value)
    {
        DemoPlugin.Config.Text = value;
        DemoPlugin.SaveConfig();
    }
}
