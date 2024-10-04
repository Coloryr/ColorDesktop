using Avalonia.Controls;
using Avalonia.Input;

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
        if (DataContext is PGItemModel model && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
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