using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.TimerPlugin;

public partial class TimerControl : UserControl, IInstance
{
    public TimerControl()
    {
        InitializeComponent();
    }

    public TimerControl(InstanceDataObj obj)
    {
        InitializeComponent();

        var model = new TimerModel(obj);
        model.PropertyChanged += Model_PropertyChanged;
        DataContext = model;
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == TimerModel.NameTop)
        {
            ScrollViewer1.ScrollToHome();
        }
    }

    public Control CreateView()
    {
        return this;
    }

    public IInstanceHandel? GetHandel()
    {
        return null;
    }

    public void RenderTick(IInstanceWindow window)
    {
        if (DataContext is TimerModel model)
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
        
    }

    public void WindowLoaded(IInstanceWindow window)
    {
        
    }
}