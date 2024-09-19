using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Manager;
using ColorDesktop.Launcher.UI.Models.Main;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.Launcher.UI.Models.Items;

public partial class InstanceItemModel : ObservableObject
{
    public string Nick => _obj.Nick;
    public string Plugin => _obj.Plugin;
    public string UUID => _obj.UUID;

    [ObservableProperty]
    private bool _enable;

    private readonly InstanceDataObj _obj;
    private readonly MainViewModel _model;

    public InstanceItemModel(MainViewModel model, InstanceDataObj obj)
    {
        _obj = obj;
        _model = model;
        _enable = ConfigHelper.Config.EnableInstance.Contains(_obj.UUID);
    }

    partial void OnEnableChanged(bool value)
    {
        if (value)
        {
            InstanceManager.EnableInstance(_obj);
        }
        else
        {
            InstanceManager.DisableInstance(_obj);
        }
    }

    [RelayCommand]
    public void OpenSetting()
    {
        InstanceManager.OpenSetting(_obj);
    }
}
