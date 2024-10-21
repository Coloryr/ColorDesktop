using Avalonia.Controls;
using Avalonia.Input;

namespace ColorDesktop.BmPlugin.Skin2;

public partial class BmSkin2Control : UserControl
{
    public BmSkin2Control()
    {
        InitializeComponent();

        Head.PointerPressed += Head_PointerPressed;
    }

    private void Head_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        (VisualRoot as Window)?.BeginMoveDrag(e);
    }
}