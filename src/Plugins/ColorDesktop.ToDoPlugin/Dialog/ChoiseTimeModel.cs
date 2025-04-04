﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.ToDoPlugin.Dialog;

public partial class ChoiseTimeModel(string name) : ObservableObject
{
    [ObservableProperty]
    private DateTimeOffset _time;

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
