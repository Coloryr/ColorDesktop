using ColorDesktop.Api.Objs;

namespace ColorDesktop.Launcher.UI.Models.Dialog;

public partial class CreateInstanceOneModel(InstanceDataObj obj) : CreateInstanceBaseModel(obj)
{
    public string Temp { get; init; } = "";
}
