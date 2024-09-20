using Avalonia.Controls;
using ColorDesktop.Api;

namespace ColorDesktop.Launcher.UI.Models.Dialog;

public partial class CreateInstanceOtherModel : CreateInstanceBaseModel
{
    public Control Control { get; init; }

    public CreateInstanceOtherModel(InstanceDataObj obj, Control control) : base(obj)
    {
        Control = control;
    }
}
