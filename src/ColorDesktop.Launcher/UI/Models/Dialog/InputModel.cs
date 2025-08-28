using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.Launcher.UI.Models.Dialog;

public partial class InputModel : ChoiseBaseModel
{
    [ObservableProperty]
    private string _input;

    [ObservableProperty]
    private string _text;
}
