using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.MonitorPlugin;

public partial class MonitorInstanceSettingModel : ObservableObject
{
    private readonly MonitorInstanceObj _config;
    private readonly InstanceDataObj _obj;

    public MonitorInstanceSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = MonitorPlugin.GetConfig(obj);
    }
}
