using ColorDesktop.Launcher.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
}
