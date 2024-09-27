using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ColorDesktop.WeatherPlugin;

public partial class WeatherSettingControl : UserControl
{
    public WeatherSettingControl()
    {
        InitializeComponent();

        DataContext = new WeatherSettingModel();
    }
}