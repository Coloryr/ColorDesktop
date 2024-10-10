using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using ColorDesktop.Api;

namespace ColorDesktop.BmPlugin;

public partial class BmControl : UserControl, IInstance
{
    public BmControl()
    {
        InitializeComponent();

        DataContext = new BmModel();
    }

    public Control CreateView()
    {
        return this;
    }

    public void RenderTick()
    {
        if (DataContext is BmModel model)
        {
            model.Tick();
        }
    }

    public void Start(IInstanceWindow window)
    {
        if (window is Window window1)
        {
            window1.PointerEntered += Window1_PointerEntered;
            window1.PointerExited += Window1_PointerExited;
        }

        if (DataContext is BmModel model)
        {
            model.Load();
        }
    }

    public void Stop(IInstanceWindow window)
    {
        if (window is Window window1)
        {
            window1.PointerEntered -= Window1_PointerEntered;
            window1.PointerExited -= Window1_PointerExited;
        }
    }

    public void Update(InstanceDataObj obj)
    {
        
    }

    private void Window1_PointerExited(object? sender, PointerEventArgs e)
    {
        if (DataContext is BmModel model)
        {
            model.IsOver = false;
        }
    }

    private void Window1_PointerEntered(object? sender, PointerEventArgs e)
    {
        if (DataContext is BmModel model)
        {
            model.IsOver = true;
        }
    }
}