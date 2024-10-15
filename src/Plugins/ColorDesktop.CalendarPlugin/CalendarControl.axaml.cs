using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Input;
using ColorDesktop.Api;
using ColorDesktop.CalendarPlugin.Skin1;
using ColorDesktop.CalendarPlugin.Skin2;

namespace ColorDesktop.CalendarPlugin;

public partial class CalendarControl : UserControl, IInstance
{
    public CalendarControl()
    {
        InitializeComponent();

        DataContext = new CalendarModel();
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

    public void Start(IInstanceWindow window)
    {
        if (window is Window window1)
        {
            window1.PointerEntered += CalendarControl_PointerEntered;
            window1.PointerExited += CalendarControl_PointerExited;
        }
    }

    public void Stop(IInstanceWindow window)
    {
        if (window is Window window1)
        {
            window1.PointerEntered -= CalendarControl_PointerEntered;
            window1.PointerExited -= CalendarControl_PointerExited;
        }
    }

    public void Update(InstanceDataObj obj)
    {
        var config = CalendarPlugin.GetConfig(obj);
        if (DataContext is CalendarModel model)
        {
            model.Update(config);
        }

        if (config.Skin == SkinType.Skin1
            && View.Child is not Skin1Control)
        {
            View.Child = new Skin1Control();
        }
        else if (config.Skin == SkinType.Skin2
            && View.Child is not Skin2Control)
        {
            View.Child = new Skin2Control();
        }
    }
}