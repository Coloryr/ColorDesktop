using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;

namespace PluginDemo;

public partial class DemoControl : UserControl, IInstance
{
    public DemoControl()
    {
        InitializeComponent();

        DataContext = new DemoModel();
    }

    public Control CreateView()
    {
        return this;
    }

    public IInstanceHandel? GetHandel()
    {
        return null;
    }

    public void RenderTick()
    {
        
    }

    public void Start(IInstanceWindow window)
    {
        
    }

    public void Stop(IInstanceWindow window)
    {
        
    }

    public void Update(InstanceDataObj obj)
    {
        var config = DemoPlugin.GetConfig(obj);
        if (DataContext is DemoModel model)
        {
            model.Update(config);
        }
    }
}