using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.MonitorPlugin.Models;

public partial class ProgressBarModel : ObservableObject, IUpdate
{
    [ObservableProperty]
    private IBrush _backColor;
    [ObservableProperty]
    private IBrush _backColor1;
    [ObservableProperty]
    private IBrush _textColor;
    [ObservableProperty]
    private IBrush _barColor;
    [ObservableProperty]
    private int _fontSize;
    [ObservableProperty]
    private double _width;
    [ObservableProperty]
    private double _height;
    [ObservableProperty]
    private Thickness _borderSize;

    [ObservableProperty]
    private double _min;
    [ObservableProperty]
    private double _max;
    [ObservableProperty]
    private double _value;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _text;

    public ProgressBarModel(MonitorItemModel model)
    {
        Reload(model);
    }

    public void Reload(MonitorItemModel model)
    {
        var item = model.Obj;
        FontSize = item.FontSize;
        try
        {
            BackColor = Brush.Parse(item.Color1 ?? "#FFFFFF");
            TextColor = Brush.Parse(item.Color2 ?? "#FFFFFF");
            BarColor = Brush.Parse(item.Color3 ?? "#FFFFFF");
            BackColor1 = Brush.Parse(item.Color4 ?? "#FFFFFF");
        }
        catch
        {

        }
        Width = item.Width <= 0 ? double.NaN : item.Width;
        Height = item.Height <= 0 ? double.NaN : item.Height;
        BorderSize = new(item.BorderSize);

        Min = item.Min;
        Max = item.Max;
        Name = item.Name;
    }

    public void Update(MonitorItemModel model)
    {
        switch (model.ValueType)
        {
            case ValueType.Now:
                Value = model.Value;
                Text = model.Format;
                break;
            case ValueType.Max:
                Value = model.MaxValue;
                Text = model.FormatMax;
                break;
            case ValueType.Min:
                Value = model.MinValue;
                Text = model.FormatMin;
                break;
        }
    }
}
