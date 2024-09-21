using System;
using System.Threading.Tasks;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Manager;
using ColorDesktop.Launcher.UI.Models.Dialog;
using ColorDesktop.Launcher.UI.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher.UI.Models.Main;

public partial class MainViewModel
{
    [ObservableProperty]
    private bool _autoStart;
    [ObservableProperty]
    private bool _autoMin;

    private bool _load;

    partial void OnAutoStartChanged(bool value)
    {
        if (_load)
        {
            return;
        }

        ConfigHelper.SetAutoStart(value);
    }

    partial void OnAutoMinChanged(bool value)
    {
        ConfigHelper.SetAuto(value);
    }

    [RelayCommand]
    public void Exit()
    {
        App.Exit();
    }

    [RelayCommand]
    public void OpenDir()
    {
        PathHelper.OpenPathWithExplorer(Program.RunDir);
    }

    [RelayCommand]
    public void OpenPluginDir()
    {
        PathHelper.OpenPathWithExplorer(PluginManager.RunDir);
    }

    [RelayCommand]
    public async Task ReloadPlugin()
    {
        var res = await DialogHost.Show(new ChoiseModel()
        {
            Text = "是否要重新读取组件"
        }, MainWindow.DialogHostName);
        if (res is true)
        {
            PluginManager.Reload();
        }
    }

    [RelayCommand]
    public async Task DisableAllPlugin()
    {
        var res = await DialogHost.Show(new ChoiseModel()
        {
            Text = "是否要禁用所有组件"
        }, MainWindow.DialogHostName);
        if (res is true)
        {
            PluginManager.DisablePlugin();
        }
    }

    [RelayCommand]
    public async Task ReloadInstance()
    {
        var res = await DialogHost.Show(new ChoiseModel()
        {
            Text = "是否要重新读取实例"
        }, MainWindow.DialogHostName);
        if (res is true)
        {
            InstanceManager.Reload();
        }
    }

    [RelayCommand]
    public async Task DisableAllInstance()
    {
        var res = await DialogHost.Show(new ChoiseModel()
        {
            Text = "是否要禁用所有实例"
        }, MainWindow.DialogHostName);
        if (res is true)
        {
            InstanceManager.DisableInstance();
        }
    }

    public void Load()
    {
        _load = true;

        AutoStart = ConfigHelper.Config.AutoStart;
        AutoMin = ConfigHelper.Config.AutoMin;

        _load = false;
    }
}
