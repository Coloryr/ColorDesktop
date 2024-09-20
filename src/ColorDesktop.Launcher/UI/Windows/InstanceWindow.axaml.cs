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
    private Control _view;

    public InstanceWindow()
    {
        InitializeComponent();

        Loaded += InstanceWindow_Loaded;

        View1.PointerEntered += View1_PointerEntered;
        HoverBorder.PointerExited += View1_PointerExited;
        HoverBorder.PointerPressed += Border1_PointerPressed;
    }

    public void Update(InstanceDataObj obj)
    {
        _obj = obj;
        Dispatcher.UIThread.Post(() =>
        {
            Move();
            PositionChanged += (a, b) =>
            {
                // ��ȡ��ǰ��Ļ
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
                        obj.Display = i + 1;
                        break;
                    }
                }
                // �����µ� Margin
                obj.Margin.Left = Position.X - workArea.X;
                obj.Margin.Top = Position.Y - workArea.Y;
                obj.Margin.Right = workArea.X + workArea.Width - (Position.X + (int)Width);
                obj.Margin.Bottom = workArea.Y + workArea.Height - (Position.Y + (int)Height);

                ConfigSave.AddItem(new ConfigSaveObj()
                {
                    Name = obj.UUID + "json",
                    Local = InstanceManager.GetDataLocal(obj),
                    Obj = obj
                });
            };
        });
    }

    private void View1_PointerExited(object? sender, PointerEventArgs e)
    {
        HoverBorder.Opacity = 0;
        UIAnimation.HideAnimation.RunAsync(HoverBorder);
    }

    private void View1_PointerEntered(object? sender, PointerEventArgs e)
    {
        HoverBorder.Opacity = 1;
        UIAnimation.ShowAnimation.RunAsync(HoverBorder);
    }

    private void Border1_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }

    private void InstanceWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        // ��ʼʱ�߿�͸��
        HoverBorder.Opacity = 0;

        Render();
    }

    private void Render()
    {
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

        View1.Child = _view = instance.CreateView();
        instance.Start();

        Update(_obj);
    }

    public void Move()
    {
        // ��ȡ������ʾ������Ϣ
        var screens = Screens.All;

        Screen? targetScreen;
        if (_obj.Display != 1 && screens.Count > _obj.Display - 1)
        {
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
    }
}
