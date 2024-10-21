using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Input;

namespace ColorDesktop.BmPlugin.Skin3;

public partial class BmSkin3Control : UserControl
{
    public BmSkin3Control()
    {
        InitializeComponent();

        Head.PointerPressed += Head_PointerPressed;

        DataContextChanged += BmSkin3Control_DataContextChanged;
    }

    private void BmSkin3Control_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is Bm3Model model)
        {
            model.PropertyChanged += Model_PropertyChanged;
        }
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == BmModel.BmMoveName)
        {
            View1.ScrollToHome();
        }
    }

    private void Head_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        (VisualRoot as Window)?.BeginMoveDrag(e);
    }
}