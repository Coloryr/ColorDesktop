using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.AnalogClockPlugin;

public partial class FlipModel : ObservableObject
{
    [ObservableProperty]
    private Bitmap? _image;

    [ObservableProperty]
    private Bitmap? _image1;

    [ObservableProperty]
    private IBrush _color;
    [ObservableProperty]
    private IBrush _color1;

    [ObservableProperty]
    private int _size;
    [ObservableProperty]
    private int _size1;
    [ObservableProperty]
    private int _size2;
    [ObservableProperty]
    private int _size3;

    partial void OnSizeChanged(int value)
    {
        Size1 = value + value / 10;
        Size2 = value / 2;
        Size3 = -(value / 2);
    }

    public void SetImage(Bitmap nextTop, Bitmap nextBottom)
    {
        Image = nextTop;
        Image1 = nextBottom;
    }

    public void Clear()
    {
        OnPropertyChanged(nameof(Clear));
    }
}
