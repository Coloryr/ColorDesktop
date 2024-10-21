using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.BmPlugin.Skin3;

public partial class Bm3Model : BmModel
{
    [ObservableProperty]
    private double _width;
    [ObservableProperty]
    private double _height;

    [ObservableProperty]
    private double _width1;

    [ObservableProperty]
    private IBrush _backColor;
    [ObservableProperty]
    private IBrush _textColor;

    public Bm3Model(BmInstanceObj config)
    {
        Update(config);
    }

    protected override BmItemModel CreateModel(BmObj.ItemObj item)
    {
        return new Bm3ItemModel(item, BackColor, TextColor, Width, Height);
    }

    public override void Update(BmInstanceObj config)
    {
        Width1 = config.Width1 <= 0 ? 280 : config.Width1;
        Width = config.Width <= 0 ? double.NaN : config.Width;
        Height = config.Height <= 0 ? double.NaN : config.Height;
        BackColor = Brush.Parse(config.Color1 ?? "#000000");
        TextColor = Brush.Parse(config.Color2 ?? "#FFFFFF");

        foreach (var item in BmItems)
        {
            (item as Bm3ItemModel)?.Update(BackColor, TextColor, Width, Height);
        }
    }
}
