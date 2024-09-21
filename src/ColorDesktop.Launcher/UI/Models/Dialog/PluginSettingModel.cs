using Avalonia.Controls;
using ColorDesktop.Launcher.UI.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher.UI.Models.Dialog;

public partial class PluginSettingModel : ObservableObject
{
    public required Control Control { get; init; }

    [RelayCommand]
    public void Done()
    {
        DialogHost.Close(MainWindow.DialogHostName);
    }
}
