﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Utils;
using ColorDesktop.Launcher.ViewModels.Main;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.Launcher.ViewModels.Items;

public partial class InstanceItemModel : ObservableObject
{
    public string Nick => _obj.Nick;
    public string Plugin => _obj.Plugin;

    [ObservableProperty]
    private bool _enable;

    private readonly InstanceDataObj _obj;
    public InstanceItemModel(MainViewModel model, InstanceDataObj obj)
    {
        _obj = obj;
        _enable = ConfigHelper.Config.EnableInstance.Contains(_obj.UUID);
    }
}
