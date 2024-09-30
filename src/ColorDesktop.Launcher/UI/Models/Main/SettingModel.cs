using System.Threading.Tasks;
using ColorDesktop.Api;
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
    public string[] TranTypes { get; init; } = LangHelper.GetWindowTranTypeLang();

    [ObservableProperty]
    private bool _autoStart;
    [ObservableProperty]
    private bool _autoMin;

    private bool _load;

    [ObservableProperty]
    private WindowTransparencyType _type;

    partial void OnTypeChanged(WindowTransparencyType value)
    {
        ConfigHelper.SetWindowTran(value);
    }

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
            Text = App.Lang("MainWindow.Info6")
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
            Text = App.Lang("MainWindow.Info7")
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
            Text = App.Lang("MainWindow.Info8")
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
            Text = App.Lang("MainWindow.Info9")
        }, MainWindow.DialogHostName);
        if (res is true)
        {
            InstanceManager.DisableInstance();
        }
    }

    public void LoadConfig()
    {
        _load = true;

        AutoStart = ConfigHelper.Config.AutoStart;
        AutoMin = ConfigHelper.Config.AutoMin;
        Type = ConfigHelper.Config.Tran;

        _load = false;
    }
}
