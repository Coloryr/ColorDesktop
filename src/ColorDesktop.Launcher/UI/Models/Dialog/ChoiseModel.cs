using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Launcher.UI.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher.UI.Models.Dialog;

public partial class ChoiseModel : ObservableObject
{
    [ObservableProperty]
    public string _text;

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
