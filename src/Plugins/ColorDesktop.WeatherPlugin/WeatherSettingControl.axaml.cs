using Avalonia.Controls;

namespace ColorDesktop.WeatherPlugin;

public partial class WeatherSettingControl : UserControl
{
    public WeatherSettingControl()
    {
        InitializeComponent();

        DataContext = new WeatherSettingModel();
    }
}