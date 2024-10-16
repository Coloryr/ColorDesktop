using Avalonia.Controls;

namespace ColorDesktop.MonitorPlugin;

public partial class MonitorSettingControl : UserControl
{
    public MonitorSettingControl()
    {
        InitializeComponent();

        DataContext = new MonitorSettingModel();
    }
}