using Avalonia.Controls;
using Avalonia.Interactivity;
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

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);

        if (DataContext is MonitorInstanceSettingModel model)
        {
            model.Stop();
        }
    }
}