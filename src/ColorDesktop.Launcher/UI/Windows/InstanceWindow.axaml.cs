using System;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using ColorDesktop.Api;

namespace ColorDesktop.Launcher.UI.Windows;

public partial class InstanceWindow : Window
{
    private InstanceDataObj _obj;
    private IInstance _instance;
    private Control _view;

    private Animation showAnimation;
    private Animation hideAnimation;

    public InstanceWindow()
    {
        InitializeComponent();

        Loaded += InstanceWindow_Loaded;
        Border1.PointerPressed += Border1_PointerPressed;

        View1.PointerEntered += View1_PointerEntered;
        HoverBorder.PointerExited += View1_PointerExited;

        // 创建显示动画
        showAnimation = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(300),
            Children =
                {
                    new KeyFrame
                    {
                        Cue = new Cue(0),
                        Setters =
                        {
                            new Setter(Border.OpacityProperty, 0)
                        }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue(1),
                        Setters =
                        {
                            new Setter(Border.OpacityProperty, 1)
                        }
                    }
                }
        };

        // 创建隐藏动画
        hideAnimation = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(300),
            Children =
                {
                    new KeyFrame
                    {
                        Cue = new Cue(0),
                        Setters =
                        {
                            new Setter(Border.OpacityProperty, 1)
                        }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue(1),
                        Setters =
                        {
                            new Setter(Border.OpacityProperty, 0)
                        }
                    }
                }
        };
    }

    private async void View1_PointerExited(object? sender, PointerEventArgs e)
    {
        HoverBorder.Opacity = 0;
        await hideAnimation.RunAsync(HoverBorder);
    }

    private async void View1_PointerEntered(object? sender, PointerEventArgs e)
    {
        HoverBorder.Opacity = 1;
        await showAnimation.RunAsync(HoverBorder);
    }

    private void Border1_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }

    private void InstanceWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        // 初始时边框透明
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
    }
}
