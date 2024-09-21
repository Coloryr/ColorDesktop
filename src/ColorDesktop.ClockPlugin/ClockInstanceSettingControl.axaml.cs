using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

namespace ColorDesktop.ClockPlugin;

public partial class ClockInstanceSettingControl : UserControl
{
    public ClockInstanceSettingControl()
    {
        InitializeComponent();
    }

    public ClockInstanceSettingControl(InstanceDataObj obj) : this()
    {
        DataContext = new ClockInstanceSettingModel(obj);
    }
}