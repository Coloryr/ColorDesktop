using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.BmPlugin;

public partial class BmModel : ObservableObject
{
    [ObservableProperty]
    private bool _isOver;
    [ObservableProperty]
    private bool _isUpdate;

    public void Tick()
    {
        
    }

    [RelayCommand]
    public void Load()
    {
        try
        {

        }
        catch
        { 
            
        }
    }
}
