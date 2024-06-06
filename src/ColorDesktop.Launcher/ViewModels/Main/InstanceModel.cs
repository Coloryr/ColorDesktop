using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Launcher.ViewModels.Items;

namespace ColorDesktop.Launcher.ViewModels.Main;

public partial class MainViewModel
{
    public ObservableCollection<InstanceItemModel> Instances { get; init; } = [];
}
