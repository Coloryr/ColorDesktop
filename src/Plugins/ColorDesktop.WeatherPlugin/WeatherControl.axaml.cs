using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

namespace ColorDesktop.WeatherPlugin;

public partial class WeatherControl : UserControl, IInstance
{
    public WeatherControl()
    {
        InitializeComponent();

        DataContext = new WeatherModel();
    }

    public Control CreateView()
    {
        return this;
    }

    public void RenderTick()
    {
        if (DataContext is WeatherModel model)
        {
            model.Tick();
        }
    }

    public void Start()
    {
        
    }

    public void Stop()
    {
        
    }

    public void Update(InstanceDataObj obj)
    {
        var config = WeatherPlugin.GetConfig(obj);
        if (DataContext is WeatherModel model)
        {
            model.Update(config);
        }
    }
}