using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Manager;
using ColorDesktop.Launcher.UI.Models.Dialog;
using ColorDesktop.Launcher.UI.Models.Main;
using ColorDesktop.Launcher.UI.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher.UI.Models.Items;

public partial class InstanceItemModel : ObservableObject
{
    public string Nick => _obj.Nick;
    public string Plugin => _obj.Plugin;
    public string UUID => _obj.UUID;

    [ObservableProperty]
    private bool _enable;
    [ObservableProperty]
    private bool _enableFail;
    [ObservableProperty]
    private bool _pluginDisable;

    private readonly InstanceDataObj _obj;
    private readonly MainViewModel _model;

    private bool _edit;
    private bool _work;

    public InstanceItemModel(MainViewModel model, InstanceDataObj obj)
    {
        _obj = obj;
        _model = model;
        _enable = InstanceManager.IsEnable(_obj.UUID);
        _enableFail = InstanceManager.IsEnableFail(_obj.UUID);
        _pluginDisable = !PluginManager.IsEnable(_obj.Plugin);
    }

    async partial void OnEnableChanged(bool value)
    {
        if (_edit || _work)
        {
            return;
        }

        _work = true;
        if (value)
        {
            var res = await DialogHost.Show(new ChoiseModel()
            { 
                Text = "是否要启用该实例"
            },MainWindow.DialogHostName);

            if (res is not true)
            {
                _edit = true;
                Enable = false;
                _edit = false;
                _work = false;
                return;
            }

            InstanceManager.EnableInstance(_obj);
        }
        else
        {
            var res = await DialogHost.Show(new ChoiseModel()
            {
                Text = "是否要禁用该实例"
            }, MainWindow.DialogHostName);

            if (res is not true)
            {
                _edit = true;
                Enable = true;
                _edit = false;
                _work = false;
                return;
            }

            InstanceManager.DisableInstance(_obj);
        }
        _work = false;
    }

    [RelayCommand]
    public void OpenSetting()
    {
        InstanceManager.OpenSetting(_obj);
    }

    [RelayCommand]
    public void Delete()
    {
        _model.Delete(this);
    }

    public void Update()
    {
        Enable = InstanceManager.IsEnable(_obj.UUID);
        EnableFail = InstanceManager.IsEnableFail(_obj.UUID);
        PluginDisable = !PluginManager.IsEnable(_obj.Plugin);
    }
}
