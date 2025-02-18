using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.Threading;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.Launcher.Hook;
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
    private PixelRect _lastSize;

    private CancellationTokenSource _cancel = new();

    public InstanceWindow()
    {
        InitializeComponent();

        Loaded += InstanceWindow_Loaded;
        Closed += InstanceWindow_Closed;

        PointerEntered += InstanceWindow_PointerEntered;
        PointerExited += InstanceWindow_PointerExited;
        PropertyChanged += InstanceWindow_PropertyChanged;
        Resized += InstanceWindow_Resized;

        HoverBorder.PointerPressed += HoverBorder_PointerPressed;

        Screens.Changed += Screens_Changed;
    }

    private void Screens_Changed(object? sender, EventArgs e)
    {
        Move();
    }

    private void InstanceWindow_Resized(object? sender, WindowResizedEventArgs e)
    {
        Dispatcher.UIThread.Post(Move);
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
        else if (e.Property == SizeToContentProperty)
        {
            if (SizeToContent != SizeToContent.WidthAndHeight)
            {
                SizeToContent = SizeToContent.WidthAndHeight;
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

        Topmost = _obj.TopModel;

        if (SystemInfo.Os == OsType.Windows)
        {
            Win32.SetTabGone(this);
        }
        else
        {
            ShowInTaskbar = false;
        }

        if (SystemInfo.Os == OsType.Windows)
        {
            Win32.SetMouseThrough(this, _obj.MouseThrough);
        }
        else if (SystemInfo.Os == OsType.Linux)
        {
            Linux.SetMouseThrough(this, _obj.MouseThrough);
        }
        else
        {

        }
        SetTran(_obj.Tran);

        Dispatcher.UIThread.Post(Move);
    }

    private void InstanceWindow_PointerExited(object? sender, PointerEventArgs e)
    {
        _cancel.Cancel();
        _cancel.Dispose();

        _cancel = new();

        _display = false;
        HoverBorder.Opacity = 0;
        UIAnimation.HideAnimation.RunAsync(HoverBorder, _cancel.Token);
    }

    private void InstanceWindow_PointerEntered(object? sender, PointerEventArgs e)
    {
        _cancel.Cancel();
        _cancel.Dispose();

        _cancel = new();

        _time = DateTime.Now;
        _display = true;
        HoverBorder.Opacity = 1;
        UIAnimation.ShowAnimation.RunAsync(HoverBorder, _cancel.Token);
    }

    private void HoverBorder_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }

    private void InstanceWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        Update(_obj);

        // 初始时边框透明
        HoverBorder.Opacity = 0;

        if (_instance == null)
        {
            return;
        }

        _instance.WindowLoaded(this);

        var screen = GetTargetScreen();
        if (screen != null)
        {
            _lastSize = screen.WorkingArea;
        }

        PositionChanged += InstanceWindow_PositionChanged;

        Render();
    }

    private void InstanceWindow_PositionChanged(object? sender, PixelPointEventArgs e)
    {
        if (_update || _obj == null)
        {
            return;
        }
        Dispatcher.UIThread.Post(() =>
        {
            // 获取当前屏幕
            var screen = GetTargetScreen();
            if (screen == null)
            {
                return;
            }
            var workArea = screen.WorkingArea;
            if (workArea != _lastSize)
            {
                _lastSize = workArea;
                Move();
                return;
            }
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
        });
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
            if (less.TotalSeconds > 10)
            {
                InstanceWindow_PointerExited(null, null!);
            }
        }
        GetTopLevel(this)?.RequestAnimationFrame((t) =>
        {
            _instance.RenderTick(this);
            Render();
        });
    }

    public void Load(IInstance instance, InstanceDataObj obj)
    {
        _obj = obj;
        _instance = instance;

        View1.Child = instance.CreateView();
        instance.Start(this);

        _instance.Update(obj);
    }

    public void Move()
    {
        _update = true;
        // 获取所有显示器的信息

        var targetScreen = GetTargetScreen();

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

    private Screen? GetTargetScreen()
    {
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

        return targetScreen;
    }

    public void Move(int x, int y)
    {
        Position = new(x, y);
    }

    public void SetState(WindowState state)
    {
        WindowState = state;
    }

    public void SetTran(WindowTransparencyType level)
    {
        switch (level)
        {
            case WindowTransparencyType.None:
                TransparencyLevelHint = [WindowTransparencyLevel.None];
                break;
            case WindowTransparencyType.Transparent:
                TransparencyLevelHint = [WindowTransparencyLevel.Transparent];
                break;
            case WindowTransparencyType.Blur:
                TransparencyLevelHint = [WindowTransparencyLevel.Blur];
                break;
            case WindowTransparencyType.AcrylicBlur:
                TransparencyLevelHint = [WindowTransparencyLevel.AcrylicBlur];
                break;
            case WindowTransparencyType.Mica:
                TransparencyLevelHint = [WindowTransparencyLevel.Mica];
                break;
        }
    }
}
