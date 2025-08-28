using ColorDesktop.Api;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.UI.Models.Dialog;
using ColorDesktop.Launcher.UI.Windows;
using ColorDesktop.Launcher.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher.UI.Models.Main;

public partial class MainViewModel : ObservableObject
{
    public string Version => Program.Version;

    [ObservableProperty]
    private int _nowView = -1;

    partial void OnNowViewChanged(int value)
    {
        if (NowView == 0)
        {
            LoadConfig();
        }
        else if (NowView == 1)
        {
            LoadPluginData();
        }
        else if (NowView == 2)
        {
            LoadInstanceData();
        }
        else if (NowView == 3)
        {
            GetDownloadList();
        }
    }

    [RelayCommand]
    public void OpenGithub()
    {
        LauncherUtils.OpUrl("https://github.com/Coloryr/ColorDesktop");
    }

    [RelayCommand]
    public void OpenLog()
    {
        LauncherUtils.OpUrl("https://github.com/Coloryr/ColorDesktop/blob/master/log.md");
    }

    [RelayCommand]
    public void OpenDownload()
    {
        NowView = 3;
    }

    public async void GoEula()
    {
        var res = await DialogHost.Show(new ChoiseModel()
        {
            Text = LangApi.GetLang("MainWindow.Text61")
        }, MainWindow.DialogHostName);
        if (res is not true)
        {
            App.Exit();
        }
        ConfigHelper.SetEula();
        await DialogHost.Show(new ChoiseModel()
        {
            Text = LangApi.GetLang("MainWindow.Text62"),
            HaveCancel = false
        }, MainWindow.DialogHostName);
    }
}
