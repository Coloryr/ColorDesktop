﻿using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.Immutable;
using ColorDesktop.CoreLib;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.BmPlugin.Skin3;

public partial class Bm3ItemModel : BmItemModel
{
    [ObservableProperty]
    private IBrush _backColor;
    [ObservableProperty]
    private IBrush _textColor;

    [ObservableProperty]
    private double _width;
    [ObservableProperty]
    private double _height;

    private IBrush _color1;
    private IBrush _color2;

    private readonly string _url;

    public Task<Bitmap?> Image => GetImage();

    public Bm3ItemModel(BmObj.ItemObj item, IBrush backcolor, IBrush textcolor, double width, double height) : base(item)
    {
        Update(backcolor, textcolor, width, height);

        if (item.Images != null)
        {
            if (width < 78)
            {
                _url = item.Images.Small ?? item.Images.Grid;
            }
            else if (width < 100)
            {
                _url = item.Images.Medium ?? item.Images.Grid;
            }
            else if (width < 150)
            {
                _url = item.Images.Common ?? item.Images.Grid;
            }
            else
            {
                _url = item.Images.Large ?? item.Images.Grid;
            }
        }
    }

    private async Task<Bitmap?> GetImage()
    {
        if (_url == null)
        {
            return BmPlugin.Icon;
        }
        var image = await TempManager.LoadImage(_url);
        return image ?? BmPlugin.Icon1;
    }

    public void Update(IBrush backcolor, IBrush textcolor, double width, double height)
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
        Width = width;
        Height = height;
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
