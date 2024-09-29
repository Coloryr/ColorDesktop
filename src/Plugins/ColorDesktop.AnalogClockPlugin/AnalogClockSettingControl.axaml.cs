using Avalonia.Controls;
using ColorDesktop.Api;

namespace ColorDesktop.AnalogClockPlugin;

public partial class AnalogClockSettingControl : UserControl
{
    public AnalogClockSettingControl()
    {
        InitializeComponent();
    }

    public AnalogClockSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new AnalogClockSettingModel(obj);
    }
}