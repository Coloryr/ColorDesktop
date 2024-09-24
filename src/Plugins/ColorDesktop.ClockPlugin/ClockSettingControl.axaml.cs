using Avalonia.Controls;

namespace ColorDesktop.ClockPlugin;

public partial class ClockSettingControl : UserControl
{
    public ClockSettingControl()
    {
        InitializeComponent();

        DataContext = new ClockSettingModel();
    }
}
