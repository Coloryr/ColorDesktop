using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

namespace ColorDesktop.WeatherPlugin;

public partial class WeatherSettingControl : UserControl
{
    public WeatherSettingControl()
    {
        InitializeComponent();
    }

    public WeatherSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new WeatherSettingModel(obj);
    }
}