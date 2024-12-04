using Avalonia.Controls;
using ColorDesktop.Api.Objs;

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