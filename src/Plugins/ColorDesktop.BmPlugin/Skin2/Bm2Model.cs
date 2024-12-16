using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.BmPlugin.Skin2;

public partial class Bm2Model : BmModel
{
    [ObservableProperty]
    private double _width;
    [ObservableProperty]
    private double _height;

    [ObservableProperty]
    private IBrush _backColor;
    [ObservableProperty]
    private IBrush _textColor;

    public Bm2Model(BmInstanceObj config)
    {
        Update(config);
    }

    protected override BmItemModel CreateModel(BmObj.ItemObj item)
    {
        return new Bm2ItemModel(item, BackColor, TextColor);
    }

    public override void Update(BmInstanceObj config)
    {
        Width = config.Width <= 0 ? double.NaN : config.Width;
        Height = config.Height <= 0 ? double.NaN : config.Height;
        try
        {
            BackColor = Brush.Parse(config.Color1 ?? "#000000");
            TextColor = Brush.Parse(config.Color2 ?? "#FFFFFF");
        }
        catch
        {

        }

        foreach (var item in BmItems)
        {
            (item as Bm2ItemModel)?.Update(BackColor, TextColor);
        }
    }
}
