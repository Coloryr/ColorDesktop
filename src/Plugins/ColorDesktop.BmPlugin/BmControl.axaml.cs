using Avalonia.Controls;
using Avalonia.Input;
using ColorDesktop.Api;
using ColorDesktop.BmPlugin.Skin1;
using ColorDesktop.BmPlugin.Skin2;
using ColorDesktop.BmPlugin.Skin3;

namespace ColorDesktop.BmPlugin;

public partial class BmControl : UserControl, IInstance
{
    public BmControl()
    {
        InitializeComponent();
    }

    public Control CreateView()
    {
        return this;
    }

    public void RenderTick()
    {
        (View1.Child?.DataContext as BmModel)?.Tick();
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
        var config = BmPlugin.GetConfig(obj);
        if (config.Skin == SkinType.Skin1
             && View1.Child is not BmSkin1Control)
        {
            View1.Child = new BmSkin1Control()
            {
                DataContext = new BmModel()
            };
        }
        else if (config.Skin == SkinType.Skin2
             && View1.Child is not BmSkin2Control)
        {
            View1.Child = new BmSkin2Control()
            {
                DataContext = new Bm2Model(config)
            };
        }
        else if (config.Skin == SkinType.Skin3
             && View1.Child is not BmSkin3Control)
        {
            View1.Child = new BmSkin3Control()
            {
                DataContext = new Bm3Model(config)
            };
        }
        (View1.Child?.DataContext as BmModel)?.Init();
        (View1.Child?.DataContext as BmModel)?.Update(config);
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