using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.CalendarPlugin;

public partial class MonthModel : ObservableObject
{
    public string Month { get; init; }



    public MonthModel(int month)
    { 
        
    }
}
