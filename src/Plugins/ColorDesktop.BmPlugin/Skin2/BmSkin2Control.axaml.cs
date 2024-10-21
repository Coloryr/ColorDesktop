using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Input;

namespace ColorDesktop.BmPlugin.Skin2;

public partial class BmSkin2Control : UserControl
{
    public BmSkin2Control()
    {
        InitializeComponent();

        Head.PointerPressed += Head_PointerPressed;

        DataContextChanged += BmSkin2Control_DataContextChanged;
    }

    private void BmSkin2Control_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is Bm2Model model)
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