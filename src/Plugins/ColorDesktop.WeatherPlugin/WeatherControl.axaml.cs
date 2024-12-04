using Avalonia.Controls;
using Avalonia.Input;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.WeatherPlugin;

public partial class WeatherControl : UserControl, IInstance
{
    public WeatherControl()
    {
        InitializeComponent();

        DataContext = new WeatherModel();
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

    public void Start(IInstanceWindow window)
    {
        if (window is Window window1)
        {
            window1.PointerEntered += WeatherControl_PointerEntered;
            window1.PointerExited += WeatherControl_PointerExited;
        }
    }

    public void Stop(IInstanceWindow window)
    {
        if (window is Window window1)
        {
            window1.PointerEntered -= WeatherControl_PointerEntered;
            window1.PointerExited -= WeatherControl_PointerExited;
        }
    }

    public void Update(InstanceDataObj obj)
    {
        var config = WeatherPlugin.GetConfig(obj);
        if (DataContext is WeatherModel model)
        {
            model.Update(config);
        }
    }

    public IInstanceHandel? GetHandel()
    {
        return null;
    }
}