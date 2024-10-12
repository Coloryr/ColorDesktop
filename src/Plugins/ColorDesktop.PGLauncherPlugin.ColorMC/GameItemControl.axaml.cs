using Avalonia.Controls;
using Avalonia.Input;

namespace ColorDesktop.PGLauncherPlugin.ColorMC;

public partial class GameItemControl : UserControl
{
    public GameItemControl()
    {
        InitializeComponent();

        PointerEntered += PGItemControl_PointerEntered;
        PointerExited += PGItemControl_PointerExited;
        DoubleTapped += GameItemControl_DoubleTapped;
    }

    private void GameItemControl_DoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is GameItemModel model)
        {
            model.Launch();
        }
    }

    private void PGItemControl_PointerExited(object? sender, PointerEventArgs e)
    {
        if (DataContext is GameItemModel model)
        {
            model.IsOver = false;
        }
    }

    private void PGItemControl_PointerEntered(object? sender, PointerEventArgs e)
    {
        if (DataContext is GameItemModel model)
        {
            model.IsOver = true;
        }
    }
}