using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ColorDesktop.MonitorPlugin;

public partial class MonitorSettingControl : UserControl
{
    public MonitorSettingControl()
    {
        InitializeComponent();

        DataContext = new MonitorSettingModel();
    }
}