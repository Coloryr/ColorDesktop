using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

namespace ColorDesktop.WeatherPlugin;

public partial class WeatherControl : UserControl, IInstance
{
    public WeatherControl()
    {
        InitializeComponent();

        DataContext = new WeatherModel();

        PointerEntered += WeatherControl_PointerEntered;
        PointerExited += WeatherControl_PointerExited;
    }

    private void WeatherControl_PointerExited(object? sender, PointerEventArgs e)
    {
        if (DataContext is WeatherModel model)
        {
            model.ShowButton = false;
        }
    }

    private void WeatherControl_PointerEntered(object? sender, PointerEventArgs e)
    {
        if (DataContext is WeatherModel model)
        {
            model.ShowButton = true;
        }
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