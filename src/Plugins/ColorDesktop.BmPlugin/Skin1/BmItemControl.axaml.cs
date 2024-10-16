using Avalonia.Controls;
using Avalonia.Input;

namespace ColorDesktop.BmPlugin.Skin1;

public partial class BmItemControl : UserControl
{
    public BmItemControl()
    {
        InitializeComponent();

        PointerPressed += BmItemControl_PointerPressed;
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