using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.ClockPlugin;

public partial class ClockControl : UserControl, IInstance
{
    public ClockControl()
    {
        InitializeComponent();

        DataContext = new ClockModel();
    }

    public void Start(IInstanceWindow window)
    {

    }

    public void Stop(IInstanceWindow window)
    {

    }

    public void RenderTick()
    {
        if (DataContext is ClockModel model)
        {
            model.Tick();
        }
    }

    public Control CreateView()
    {
        return this;
    }

    public void Update(InstanceDataObj obj)
    {
        var config = ClockPlugin.GetConfig(obj);
        if (DataContext is ClockModel model)
        {
            model.Update(config);
        }
    }

    public IInstanceHandel? GetHandel()
    {
        return null;
    }

    public void WindowLoaded()
    {
        RenderTick();
    }
}
