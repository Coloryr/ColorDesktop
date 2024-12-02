using Avalonia.Controls;
using Avalonia.Input;
using ColorDesktop.Api;

namespace ColorDesktop.ToDoPlugin;

public partial class ToDoControl : UserControl, IInstance
{
#if DEBUG
    public ToDoControl()
    {
        InitializeComponent();
    }
#endif
    public ToDoControl(string uuid)
    {
        InitializeComponent();

        Head.PointerPressed += Head_PointerPressed;

        DataContext = new ToDoModel(uuid);
    }

    private void Head_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (VisualRoot is Window window)
        {
            window.BeginMoveDrag(e);
        }
    }

    public Control CreateView()
    {
        return this;
    }

    public void RenderTick()
    {

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
        var config = ToDoPlugin.GetConfig(obj);
        Width = config.Width;
        Height = config.Height <= 0 ? double.NaN : config.Height;
        if (DataContext is ToDoModel model)
        {
            model.Update(obj, config);
        }
    }

    private void Window1_PointerExited(object? sender, PointerEventArgs e)
    {
        if (DataContext is ToDoModel model)
        {
            model.DisplayButton = false;
        }
    }

    private void Window1_PointerEntered(object? sender, PointerEventArgs e)
    {
        if (DataContext is ToDoModel model)
        {
            model.DisplayButton = true;
        }
    }
}