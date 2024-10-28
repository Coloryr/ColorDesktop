using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.ToDoPlugin.Dialog;

public partial class ChoiseModel(string name) : ObservableObject
{
    [ObservableProperty]
    public string _text;
    [ObservableProperty]
    private bool _haveCancel = true;

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
