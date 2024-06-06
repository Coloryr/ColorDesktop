using Avalonia.Controls;

namespace ColorDesktop.ClockPlugin;

public partial class ClockSettingWindow : Window
{
    public ClockSettingWindow()
    {
        InitializeComponent();

        Closed += ClockSettingWindow_Closed;
    }

    private void ClockSettingWindow_Closed(object? sender, EventArgs e)
    {
        ClockPlugin.ClockSetting = null;
    }
}
