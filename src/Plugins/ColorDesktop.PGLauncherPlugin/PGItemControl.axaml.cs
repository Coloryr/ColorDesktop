using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace ColorDesktop.PGLauncherPlugin;

public partial class PGItemControl : UserControl
{
    public PGItemControl()
    {
        InitializeComponent();

        PointerEntered += PGItemControl_PointerEntered;
        PointerExited += PGItemControl_PointerExited;
        PointerPressed += PGItemControl_PointerPressed;
    }

    private void PGItemControl_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is PGItemModel model)
        {
            model.Launch();
        }
    }

    private void PGItemControl_PointerExited(object? sender, PointerEventArgs e)
    {
        if (DataContext is PGItemModel model)
        {
            model.IsOver = false;
        }
    }

    private void PGItemControl_PointerEntered(object? sender, PointerEventArgs e)
    {
        if (DataContext is PGItemModel model)
        {
            model.IsOver = true;
        }
    }
}