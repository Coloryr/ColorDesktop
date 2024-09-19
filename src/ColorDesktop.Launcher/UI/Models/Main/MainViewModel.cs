using ColorDesktop.Launcher.Desktop;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.Launcher.UI.Models.Main;

public partial class MainViewModel : ObservableObject
{
    public string Version => Program.Version;

    [ObservableProperty]
    private int _nowView = -1;

    partial void OnNowViewChanged(int value)
    {
        if (NowView == 1)
        {
            LoadPluginData();
        }
        else if (NowView == 2)
        {
            LoadInstanceData();
        }
    }
}
