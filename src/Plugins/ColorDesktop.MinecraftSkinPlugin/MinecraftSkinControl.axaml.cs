using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

namespace ColorDesktop.MinecraftSkinPlugin;

public partial class MinecraftSkinControl : UserControl, IInstance
{
    private bool _lowFps;
    private bool _lastTick = false;
    private DateTime _time;

    public MinecraftSkinControl()
    {
        InitializeComponent();

        DataContext = new MinecraftSkinModel();
    }

    public Control CreateView()
    {
        return this;
    }

    public void RenderTick()
    {
        if (_time.Ticks == 0)
        {
            _time = DateTime.Now;
        }
        else
        {
            var time = DateTime.Now;
            var less = time - _time;
            if (less.TotalSeconds > 1)
            {
                _time = time;
                Fps.Text = Skin.Fps.ToString();
                Skin.Fps = 0;
            }
        }
        if (_lowFps)
        {
            if (!_lastTick)
            {
                Skin.RequestNextFrameRendering();
            }
            _lastTick = !_lastTick;
        }
        else
        {
            Skin.RequestNextFrameRendering();
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
        var config = MinecraftSkinPlugin.GetConfig(obj);
        _lowFps = config.LowFps;
        Width = config.Width;
        Height = config.Height;
        FpsView.IsVisible = config.DisplayFps;
        Skin.Update(config);
    }

    private void Window1_PointerExited(object? sender, PointerEventArgs e)
    {
        if (DataContext is MinecraftSkinModel model)
        {
            model.DisplayButton = false;
        }
    }

    private void Window1_PointerEntered(object? sender, PointerEventArgs e)
    {
        if (DataContext is MinecraftSkinModel model)
        {
            model.DisplayButton = true;
        }
    }
}