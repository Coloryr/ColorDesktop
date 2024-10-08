using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace ColorDesktop.BmPlugin;

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