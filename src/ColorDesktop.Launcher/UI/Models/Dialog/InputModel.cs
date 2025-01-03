using ColorDesktop.Launcher.UI.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher.UI.Models.Dialog;

public partial class InputModel : ChoiseBaseModel
{
    [ObservableProperty]
    private string _input;

    [ObservableProperty]
    private string _text;
}
