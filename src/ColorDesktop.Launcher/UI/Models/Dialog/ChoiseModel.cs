using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.Launcher.UI.Models.Dialog;

public partial class ChoiseModel : ChoiseBaseModel
{
    [ObservableProperty]
    private string _text;
}
