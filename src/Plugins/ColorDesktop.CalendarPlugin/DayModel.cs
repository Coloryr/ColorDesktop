﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Lunar;

namespace ColorDesktop.CalendarPlugin;

public partial class DayModel : ObservableObject
{
    public int Day { get; init; }
    public string Lunar { get; init; }
    public IBrush Color { get; init; }

    private readonly Solar _solar;

    public DayModel(int year, int month, int day, IBrush color)
    {
        Color = color;
        Day = day;
        _solar = new(year, month, day);

        var temp = _solar.Lunar;
        if (!string.IsNullOrWhiteSpace(temp.JieQi))
        {
            Lunar = temp.JieQi;
        }
        else if (temp.Day == 1)
        {
            Lunar = temp.MonthInChinese + "月";
        }
        else
        {
            Lunar = temp.DayInChinese;
        }
    }

    public DayModel()
    { 
        
    }
}