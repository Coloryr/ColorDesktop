﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.Launcher.ViewModels;

public partial class AppModel : ObservableObject
{
    [RelayCommand]
    public void ShowWindow()
    { 
        
    }

    [RelayCommand]
    public void ShowSetting()
    {

    }

    [RelayCommand]
    public void Exit()
    { 
        
    }
}
