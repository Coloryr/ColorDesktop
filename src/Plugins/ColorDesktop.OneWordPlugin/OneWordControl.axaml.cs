using Avalonia.Controls;
using Avalonia.Input;
using ColorDesktop.Api;

namespace ColorDesktop.OneWordPlugin;

public partial class OneWordControl : UserControl, IInstance
{
    public OneWordControl()
    {
        InitializeComponent();

        DataContext = new OneWordModel();
    }

    public Control CreateView()
    {
        return this;
    }

    public void RenderTick()
    {
        if (DataContext is OneWordModel model)
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
        var config = OneWordPlugin.GetConfig(obj);
        if (DataContext is OneWordModel model)
        {
            model.Update(config);
        }
    }

    private void Window1_PointerExited(object? sender, PointerEventArgs e)
    {
        if (DataContext is OneWordModel model)
        {
            model.ShowButton = false;
        }
    }

    private void Window1_PointerEntered(object? sender, PointerEventArgs e)
    {
        if (DataContext is OneWordModel model)
        {
            model.ShowButton = true;
        }
    }
}