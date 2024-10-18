using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.BmPlugin.Skin2;

public partial class Bm2ItemModel : BmItemModel
{
    [ObservableProperty]
    private IBrush _backColor;
    [ObservableProperty]
    private IBrush _textColor;

    private IBrush _color1;
    private IBrush _color2;

    public Bm2ItemModel(BmObj.ItemObj item, IBrush backcolor, IBrush textcolor) : base(item)
    {
        Update(backcolor, textcolor);
    }

    public void Update(IBrush backcolor, IBrush textcolor)
    {
        _color1 = BackColor = backcolor;
        var hslcolor = (backcolor as ISolidColorBrush)!.Color.ToHsl();
        if (hslcolor.L > 0.5 && hslcolor.L < 0.9)
        {
            hslcolor = new HslColor(hslcolor.A, hslcolor.H, hslcolor.S, hslcolor.L + 0.1);
        }
        else if (hslcolor.L >= 0.9)
        {
            hslcolor = new HslColor(hslcolor.A, hslcolor.H, hslcolor.S, hslcolor.L - 0.1);
        }
        else if (hslcolor.L > 0.1 && hslcolor.L <= 0.5)
        {
            hslcolor = new HslColor(hslcolor.A, hslcolor.H, hslcolor.S, hslcolor.L - 0.1);
        }
        else if (hslcolor.L <= 0.1)
        {
            hslcolor = new HslColor(hslcolor.A, hslcolor.H, hslcolor.S, hslcolor.L + 0.1);
        }
        _color2 = new ImmutableSolidColorBrush(hslcolor.ToRgb());
        TextColor = textcolor;
    }

    public void SetOver(bool over)
    {
        if (over)
        {
            BackColor = _color2;
        }
        else
        {
            BackColor = _color1;
        }
    }
}
