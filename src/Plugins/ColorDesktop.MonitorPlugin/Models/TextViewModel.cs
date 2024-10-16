using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.MonitorPlugin.Models;

public partial class TextViewModel : ObservableObject, IUpdate
{
    public IBrush BackColor { get; init; }
    public IBrush TextColor { get; init; }
    public int FontSize { get; init; }
    public double Width { get; init; }
    public double Height { get; init; }

    [ObservableProperty]
    private string _text;

    public TextViewModel(MonitorItemModel model)
    {
        var item = model.Obj;
        FontSize = item.FontSize;
        BackColor = Brush.Parse(item.Color1);
        TextColor = Brush.Parse(item.Color2);
        Width = item.Width <= 0 ? double.NaN : item.Width;
        Height = item.Height <= 0 ? double.NaN : item.Height;
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
}
