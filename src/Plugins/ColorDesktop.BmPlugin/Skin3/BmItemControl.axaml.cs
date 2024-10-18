using Avalonia.Controls;
using Avalonia.Input;

namespace ColorDesktop.BmPlugin.Skin3;

public partial class BmItemControl : UserControl
{
    public BmItemControl()
    {
        InitializeComponent();

        PointerPressed += BmItemControl_PointerPressed;

        Border1.PointerEntered += Border1_PointerEntered;
        Border1.PointerExited += Border1_PointerExited;
    }

    private void Border1_PointerExited(object? sender, PointerEventArgs e)
    {
        if (DataContext is Bm3ItemModel model)
        {
            model.SetOver(false);
        }
    }

    private void Border1_PointerEntered(object? sender, PointerEventArgs e)
    {
        if (DataContext is Bm3ItemModel model)
        {
            model.SetOver(true);
        }
    }

    private void BmItemControl_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(this);
        if (point.Properties.IsLeftButtonPressed && DataContext is BmItemModel model)
        {
            model.OpenUrl();
        }
    }
}