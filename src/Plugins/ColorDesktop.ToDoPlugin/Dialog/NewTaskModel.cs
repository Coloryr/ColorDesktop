using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.ToDoPlugin.Dialog;

public partial class NewTaskModel(string name) : ObservableObject
{
    [ObservableProperty]
    private string _title;

    [RelayCommand]
    public void Confirm()
    {
        DialogHost.Close(name, true);
    }

    [RelayCommand]
    public void Cancel()
    {
        DialogHost.Close(name, false);
    }
}
