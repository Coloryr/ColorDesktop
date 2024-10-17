using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.MonitorPlugin.Models;

public partial class TextViewModel : ObservableObject, IUpdate
{
    [ObservableProperty]
    private IBrush _backColor;
    [ObservableProperty]
    private IBrush _textColor;
    [ObservableProperty]
    private int _fontSize;
    [ObservableProperty]
    private double _width;
    [ObservableProperty]
    private double _height;
    [ObservableProperty]
    private Thickness _borderSize;

    [ObservableProperty]
    private string _text;

    public TextViewModel(MonitorItemModel model)
    {
        Reload(model);
    }

    public void Update(MonitorItemModel model)
    {
        switch (model.ValueType)
        {
            case ValueType.Now:
                Text = model.Format;
                break;
            case ValueType.Max:
                Text = model.FormatMax;
                break;
            case ValueType.Min:
                Text = model.FormatMin;
                break;
        }
    }

    public void Reload(MonitorItemModel model)
    {
        var item = model.Obj;
        FontSize = item.FontSize;
        BackColor = Brush.Parse(item.Color1);
        TextColor = Brush.Parse(item.Color2);
        Width = item.Width <= 0 ? double.NaN : item.Width;
        Height = item.Height <= 0 ? double.NaN : item.Height;
        BorderSize = new(item.BorderSize);
    }
}
