﻿using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.MonitorPlugin.Models;

public partial class ProgressBar3Model : ObservableObject, IUpdate
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

    private double _min;
    private double _max;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _text;

    [ObservableProperty]
    private double _progress;

    public ProgressBar3Model(MonitorItemModel model)
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

        _min = item.Min;
        _max = item.Max;
        Name = item.Name;
    }

    public void Update(MonitorItemModel model)
    {
        switch (model.ValueType)
        {
            case ValueType.Now:
                Progress = (model.Value - _min) / (_max - _min) * 100;
                Text = model.Format;
                break;
            case ValueType.Max:
                Progress = (model.MaxValue - _min) / (_max - _min) * 100;
                Text = model.FormatMax;
                break;
            case ValueType.Min:
                Progress = (model.MinValue - _min) / (_max - _min) * 100;
                Text = model.FormatMin;
                break;
        }
    }
}