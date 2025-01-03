using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Launcher.Manager;
using ColorDesktop.Launcher.UI.Models.Items;
using ColorDesktop.Launcher.UI.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

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
