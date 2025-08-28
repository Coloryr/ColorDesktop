using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.AnalogClockPlugin;

public partial class DigitalModel : ObservableObject
{
    [ObservableProperty]
    private string _pathA;
    [ObservableProperty]
    private string _pathB;

    [ObservableProperty]
    private IBrush _color;

    [ObservableProperty]
    private Thickness _marginA;
    [ObservableProperty]
    private Thickness _marginB;
    [ObservableProperty]
    private Thickness _marginC;
    [ObservableProperty]
    private Thickness _marginD;
    [ObservableProperty]
    private Thickness _marginE;
    [ObservableProperty]
    private Thickness _marginF;
    [ObservableProperty]
    private Thickness _marginG;

    [ObservableProperty]
    private double _dataA;
    [ObservableProperty]
    private double _dataB;
    [ObservableProperty]
    private double _dataC;
    [ObservableProperty]
    private double _dataD;
    [ObservableProperty]
    private double _dataE;
    [ObservableProperty]
    private double _dataF;
    [ObservableProperty]
    private double _dataG;

    public void SetValue(int num)
    {
        switch (num)
        {
            case 0:
                DataA = 1;
                DataB = 1;
                DataC = 1;
                DataD = 1;
                DataE = 1;
                DataF = 1;
                DataG = 0;
                break;
            case 1:
                DataA = 0;
                DataB = 1;
                DataC = 1;
                DataD = 0;
                DataE = 0;
                DataF = 0;
                DataG = 0;
                break;
            case 2:
                DataA = 1;
                DataB = 1;
                DataC = 0;
                DataD = 1;
                DataE = 1;
                DataF = 0;
                DataG = 1;
                break;
            case 3:
                DataA = 1;
                DataB = 1;
                DataC = 1;
                DataD = 1;
                DataE = 0;
                DataF = 0;
                DataG = 1;
                break;
            case 4:
                DataA = 0;
                DataB = 1;
                DataC = 1;
                DataD = 0;
                DataE = 0;
                DataF = 1;
                DataG = 1;
                break;
            case 5:
                DataA = 1;
                DataB = 0;
                DataC = 1;
                DataD = 1;
                DataE = 0;
                DataF = 1;
                DataG = 1;
                break;
            case 6:
                DataA = 1;
                DataB = 0;
                DataC = 1;
                DataD = 1;
                DataE = 1;
                DataF = 1;
                DataG = 1;
                break;
            case 7:
                DataA = 1;
                DataB = 1;
                DataC = 1;
                DataD = 0;
                DataE = 0;
                DataF = 0;
                DataG = 0;
                break;
            case 8:
                DataA = 1;
                DataB = 1;
                DataC = 1;
                DataD = 1;
                DataE = 1;
                DataF = 1;
                DataG = 1;
                break;
            case 9:
                DataA = 1;
                DataB = 1;
                DataC = 1;
                DataD = 1;
                DataE = 0;
                DataF = 1;
                DataG = 1;
                break;
        }
    }

    public void Update(AnalogClockInstanceConfigObj obj)
    {
        try
        {
            Color = Brush.Parse(obj.Color);
        }
        catch
        {

        }

        var size = obj.Size / 10;
        var size1 = obj.Size - size;
        var size2 = size + size;
        PathA = $"M 0,{size} L {size},0 L {size1},0 L {obj.Size},{size} L {size1},{size2} L {size},{size2} Z";
        PathB = $"M 0,{size} L {size},0 L {size2},{size} L {size2},{size1} L {size},{obj.Size} L 0,{size1} Z";

        var size3 = obj.Size / 50;
        var size4 = size + size3;
        var size5 = size2 + obj.Size - size3 - size3;
        var size6 = obj.Size + obj.Size + size4 - size3 - size3;
        MarginA = new(size4, 0, size4, 0);
        MarginB = new(0, size4, 0, 0);
        MarginC = new(0, size5, 0, 0);
        MarginD = new(size4, size6, 0, 0);
        MarginE = new(0, size5, 0, 0);
        MarginF = new(0, size4, 0, 0);
        MarginG = new(size4, obj.Size + size3 + size3, 0, 0);
    }
}
