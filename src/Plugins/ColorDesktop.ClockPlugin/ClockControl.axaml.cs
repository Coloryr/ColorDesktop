using Avalonia.Controls;
using ColorDesktop.Api;

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
}
