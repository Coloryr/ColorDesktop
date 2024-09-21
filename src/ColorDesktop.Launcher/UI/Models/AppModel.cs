using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.Launcher.ViewModels;

public partial class AppModel : ObservableObject
{
    [RelayCommand]
    public void ShowWindow()
    {
        App.ShowMainWindow();
    }

    //[RelayCommand]
    //public void ShowSetting()
    //{

    //}

    [RelayCommand]
    public void Exit()
    {
        App.Exit();
    }
}
