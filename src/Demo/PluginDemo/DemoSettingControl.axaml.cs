using Avalonia.Controls;

namespace PluginDemo;

public partial class DemoSettingControl : UserControl
{
    public DemoSettingControl()
    {
        InitializeComponent();

        DataContext = new DemoSettingModel();
    }
}