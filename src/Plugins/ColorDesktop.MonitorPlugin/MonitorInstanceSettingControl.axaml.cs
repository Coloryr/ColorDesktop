
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

namespace ColorDesktop.MonitorPlugin;

public partial class MonitorInstanceSettingControl : UserControl
{
    public MonitorInstanceSettingControl()
    {
        InitializeComponent();
    }

    public MonitorInstanceSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new MonitorInstanceSettingModel(obj);
    }
}