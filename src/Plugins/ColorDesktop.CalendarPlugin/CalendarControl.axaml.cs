using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

namespace ColorDesktop.CalendarPlugin;

public partial class CalendarControl : UserControl, IInstance
{
    public CalendarControl()
    {
        InitializeComponent();

        DataContext = new CalendarModel();

        PointerEntered += CalendarControl_PointerEntered;
        PointerExited += CalendarControl_PointerExited;
    }

    private void CalendarControl_PointerExited(object? sender, PointerEventArgs e)
    {
        if (DataContext is CalendarModel model)
        {
            model.ShowButton = false;
        }
    }

    private void CalendarControl_PointerEntered(object? sender, PointerEventArgs e)
    {
        if (DataContext is CalendarModel model)
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
        if (DataContext is CalendarModel model)
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
        var config = CalendarPlugin.GetConfig(obj);
        if (DataContext is CalendarModel model)
        {
            model.Update(config);
        }
    }
}