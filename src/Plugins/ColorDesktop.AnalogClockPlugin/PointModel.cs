using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.AnalogClockPlugin;

public partial class PointModel : ObservableObject
{
    [ObservableProperty]
    private IBrush _color;

    [ObservableProperty]
    private Thickness _marginA;

    [ObservableProperty]
    private string _path;

    [ObservableProperty]
    private double _display = 1.0;

    public void SetState(bool display)
    {
        Display = display ? 1.0 : 0.0;
    }

    public void Update(AnalogClockConfigObj obj)
    {
        Color = Brush.Parse(obj.Color);

        var size = obj.Size / 5;

        MarginA = new(0, 0, 0, size * 3);

        Path = $"M 0,0 L 0,{size} L {size},{size} L {size},0 Z";
    }
}
