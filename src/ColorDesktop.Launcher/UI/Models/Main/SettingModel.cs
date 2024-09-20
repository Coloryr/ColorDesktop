using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Launcher.Helper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

    public void Load()
    {
        _load = true;

        AutoStart = ConfigHelper.Config.AutoStart;
        AutoMin = ConfigHelper.Config.AutoMin;

        _load = false;
    }
}
