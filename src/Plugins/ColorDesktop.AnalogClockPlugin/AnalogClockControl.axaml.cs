using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.AnalogClockPlugin;

public partial class AnalogClockControl : UserControl, IInstance
{
    public AnalogClockControl()
    {
        InitializeComponent();
    }

    public Control CreateView()
    {
        return this;
    }

    public void RenderTick(IInstanceWindow window)
    {
        if (View.Child is IClock control)
        {
            control.Tick();
        }
    }

    public void Start(IInstanceWindow window)
    {

    }

    public void Stop(IInstanceWindow window)
    {

    }

    public void Update(InstanceDataObj obj)
    {
        var config = AnalogClockPlugin.GetConfig(obj);
        if (config.Type == ClockType.Analog && View.Child is not PointerClockControl)
        {
            View.Child = new PointerClockControl();
        }
        else if (config.Type == ClockType.Digital && View.Child is not DigitalClockControl)
        {
            View.Child = new DigitalClockControl();
        }
        else if (config.Type == ClockType.Flip && View.Child is not FlipClockControl)
        {
            View.Child = new FlipClockControl();
        }

        if (View.Child is IClock control)
        {
            control.Update(config);
        }
    }

    public IInstanceHandel? GetHandel()
    {
        return null;
    }

    public void WindowLoaded(IInstanceWindow window)
    {
        RenderTick(window);
    }
}