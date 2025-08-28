using System.Collections.Generic;
using ColorDesktop.Launcher.Manager;
using ColorDesktop.Launcher.UI.Models.Items;

namespace ColorDesktop.Launcher.UI.Models.Dialog;

public partial class ChoiseInstanceModel : ChoiseBaseModel
{
    public List<ChoiseInstanceItemModel> Items { get; init; } = [];

    public ChoiseInstanceModel(string uuid)
    {
        var group = InstanceManager.Groups[uuid];
        foreach (var item in InstanceManager.Instances)
        {
            if (group.Instances.Contains(item.Key))
            {
                continue;
            }

            Items.Add(new(item.Value));
        }
    }
}
