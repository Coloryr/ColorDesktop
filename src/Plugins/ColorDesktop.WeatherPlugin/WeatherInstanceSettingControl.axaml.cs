using Avalonia.Controls;
using ColorDesktop.Api;

namespace ColorDesktop.WeatherPlugin;

public partial class WeatherInstanceSettingControl : UserControl
{
    public WeatherInstanceSettingControl()
    {
        InitializeComponent();
    }

    public WeatherInstanceSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new WeatherInstanceSettingModel(obj);
    }
}