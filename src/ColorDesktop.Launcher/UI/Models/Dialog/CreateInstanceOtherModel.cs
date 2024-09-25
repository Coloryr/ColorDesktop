using Avalonia.Controls;
using ColorDesktop.Api;

namespace ColorDesktop.Launcher.UI.Models.Dialog;

public partial class CreateInstanceOtherModel(InstanceDataObj obj, Control control) : CreateInstanceBaseModel(obj)
{
    public Control Control { get; init; } = control;
}
