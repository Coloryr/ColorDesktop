using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

namespace ColorDesktop.MonitorPlugin;

public partial class MonitorControl : UserControl, IInstance
{
    public MonitorControl()
    {
        InitializeComponent();

        DataContext = new MonitorModel();
    }

    public Control CreateView()
    {
        return this;
    }

    public void RenderTick()
    {
        if (DataContext is MonitorModel model)
        {
            model.Tick();
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
        var config = MonitorPlugin.GetConfig(obj);
        if (DataContext is MonitorModel model)
        {
            model.Update(config);
        }
    }
}