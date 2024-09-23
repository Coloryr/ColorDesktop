using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.Threading;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Manager;
using ColorDesktop.Launcher.Utils;

namespace ColorDesktop.Launcher.UI.Windows;

public partial class InstanceWindow : Window, IInstanceWindow
{
    private InstanceDataObj _obj;
    private IInstance _instance;
    private bool _update;
    private bool _close;
    private bool _display;

    private DateTime _time;

    public InstanceWindow()
    {
        InitializeComponent();

        Loaded += InstanceWindow_Loaded;
        Closed += InstanceWindow_Closed;

        View1.PointerEntered += View1_PointerEntered;
        HoverBorder.PointerExited += HoverBorder_PointerExited;
        HoverBorder.PointerPressed += HoverBorder_PointerPressed;
        PropertyChanged += InstanceWindow_PropertyChanged;
    }

    private void InstanceWindow_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == WindowStateProperty)
        {
            if (WindowState != WindowState.Normal)
            {
                WindowState = WindowState.Normal;
            }
        }
    }

    private void InstanceWindow_Closed(object? sender, EventArgs e)
    {
        _close = true;
    }

    public void Update(InstanceDataObj obj)
    {
        _obj = obj;
        _instance.Update(obj);
        Topmost = _obj.TopModel;
        Dispatcher.UIThread.Post(Move);
    }

    private void HoverBorder_PointerExited(object? sender, PointerEventArgs e)
    {
        _display = false;
        HoverBorder.Opacity = 0;
        UIAnimation.HideAnimation.RunAsync(HoverBorder);
    }

    private void View1_PointerEntered(object? sender, PointerEventArgs e)
    {
        _time = DateTime.Now;
        _display = true;
        HoverBorder.Opacity = 1;
        UIAnimation.ShowAnimation.RunAsync(HoverBorder);
    }

    private void HoverBorder_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }

    private void InstanceWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        // 初始时边框透明
        HoverBorder.Opacity = 0;

        _instance.RenderTick();

        PositionChanged += (a, b) =>
        {
            if (_update || _obj == null)
            {
                return;
            }
            // 获取当前屏幕
            var screen = Screens.Primary;
            if (screen == null)
            {
                return;
            }
            var workArea = screen.WorkingArea;

            for (int i = 0; i < Screens.All.Count; i++)
            {
                if (Screens.All[i] == screen)
                {
                    _obj.Display = i + 1;
                    break;
                }
            }
            // 计算新的 Margin
            _obj.Margin.Left = Position.X - workArea.X;
            _obj.Margin.Top = Position.Y - workArea.Y;
            _obj.Margin.Right = workArea.X + workArea.Width - (Position.X + (int)Width);
            _obj.Margin.Bottom = workArea.Y + workArea.Height - (Position.Y + (int)Height);

            _obj.Save();
        };

        Dispatcher.UIThread.Post(Move);

        Render();
    }

    private void Render()
    {
        if (_close)
        {
            return;
        }
        if (_display)
        {
            var less = DateTime.Now - _time;
            if(less.TotalSeconds > 10)
            {
                HoverBorder_PointerExited(null, null!);
            }
        }
        GetTopLevel(this)?.RequestAnimationFrame((t) =>
        {
            _instance.RenderTick();
            Render();
        });
    }

    public void Load(IInstance instance, InstanceDataObj obj)
    {
        _obj = obj;
        _instance = instance;

        View1.Child = instance.CreateView();
        instance.Start();

        Update(_obj);
    }

    public void Move()
    {
        _update = true;
        // 获取所有显示器的信息
        var screens = Screens.All;

        Screen? targetScreen;
        if (_obj.Display != 1 && screens.Count > _obj.Display - 1)
        {
            if (_obj.Display == 0)
            {
                _obj.Display = 1;
                _obj.Save();
            }
            targetScreen = screens[_obj.Display - 1];
        }
        else
        {
            targetScreen = Screens.Primary;
        }

        if (targetScreen != null)
        {
            var margin = _obj.Margin;
            var workArea = targetScreen.WorkingArea;
            int x = 0;
            int y = 0;

            switch (_obj.Pos)
            {
                case PosEnum.TopLeft:
                    x = workArea.X + margin.Left;
                    y = workArea.Y + margin.Top;
                    break;
                case PosEnum.Top:
                    x = workArea.X + (workArea.Width - (int)Width) / 2 + margin.Left - margin.Right;
                    y = workArea.Y + margin.Top;
                    break;
                case PosEnum.TopRight:
                    x = workArea.X + workArea.Width - (int)Width - margin.Right;
                    y = workArea.Y + margin.Top;
                    break;
                case PosEnum.Left:
                    x = workArea.X + margin.Left;
                    y = workArea.Y + (workArea.Height - (int)Height) / 2 + margin.Top - margin.Bottom;
                    break;
                case PosEnum.Center:
                    x = workArea.X + (workArea.Width - (int)Width) / 2 + margin.Left - margin.Right;
                    y = workArea.Y + (workArea.Height - (int)Height) / 2 + margin.Top - margin.Bottom;
                    break;
                case PosEnum.Right:
                    x = workArea.X + workArea.Width - (int)Width - margin.Right;
                    y = workArea.Y + (workArea.Height - (int)Height) / 2 + margin.Top - margin.Bottom;
                    break;
                case PosEnum.BottomLeft:
                    x = workArea.X + margin.Left;
                    y = workArea.Y + workArea.Height - (int)Height - margin.Bottom;
                    break;
                case PosEnum.Bottom:
                    x = workArea.X + (workArea.Width - (int)Width) / 2 + margin.Left - margin.Right;
                    y = workArea.Y + workArea.Height - (int)Height - margin.Bottom;
                    break;
                case PosEnum.BottomRight:
                    x = workArea.X + workArea.Width - (int)Width - margin.Right;
                    y = workArea.Y + workArea.Height - (int)Height - margin.Bottom;
                    break;
            }

            Position = new(x, y);
        }
        _update = false;
    }
}
