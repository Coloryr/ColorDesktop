using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Api.Objs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PluginDemo;

public partial class DemoModel : ObservableObject
{
    [ObservableProperty]
    private string _text;

    [RelayCommand]
    public void Click()
    {
        Text += "1";
    }

    public void Update(DemoObj obj)
    {
        Text = obj.Text;
    }
}
