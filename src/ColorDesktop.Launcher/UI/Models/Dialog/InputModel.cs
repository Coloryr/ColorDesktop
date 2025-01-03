using ColorDesktop.Launcher.UI.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher.UI.Models.Dialog;

public partial class InputModel : ObservableObject
{
    [ObservableProperty]
    private string _input;

    [ObservableProperty]
    private string _text;
    [ObservableProperty]
    private bool _haveCancel = true;

    [RelayCommand]
    public void Confirm()
    {
        DialogHost.Close(MainWindow.DialogHostName, true);
    }

    [RelayCommand]
    public void Cancel()
    {
        DialogHost.Close(MainWindow.DialogHostName, false);
    }
}
