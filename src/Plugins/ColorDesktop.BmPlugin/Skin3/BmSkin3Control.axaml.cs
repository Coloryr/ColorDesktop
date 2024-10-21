using Avalonia.Controls;
using Avalonia.Input;

namespace ColorDesktop.BmPlugin.Skin3;

public partial class BmSkin3Control : UserControl
{
    public BmSkin3Control()
    {
        InitializeComponent();

        Head.PointerPressed += Head_PointerPressed;
    }

    private void Head_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        (VisualRoot as Window)?.BeginMoveDrag(e);
    }
}