using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.CalendarPlugin;

public partial class MonthModel : ObservableObject
{
    public ObservableCollection<DayModel> Days { get; init; } = [];

    public IBrush Color { get; init; }

    public string Week1 { get; init; }
    public string Week2 { get; init; }
    public string Week3 { get; init; }
    public string Week4 { get; init; }
    public string Week5 { get; init; }
    public string Week6 { get; init; }
    public string Week7 { get; init; }

    public MonthModel(int year, int month, IBrush color, WeekStart weekstart)
    {
        Color = color;
        int dt = DateTime.DaysInMonth(year, month);
        var week = (int)new DateTime(year, month, 1).DayOfWeek;
        switch (weekstart)
        {
            case WeekStart.DaySun:
                Week1 = "日";
                Week2 = "一";
                Week3 = "二";
                Week4 = "三";
                Week5 = "四";
                Week6 = "五";
                Week7 = "六";
                break;
            case WeekStart.DaySat:
                Week1 = "六";
                Week2 = "日";
                Week3 = "一";
                Week4 = "二";
                Week5 = "三";
                Week6 = "四";
                Week7 = "五";
                week += 1;
                break;
            case WeekStart.DayOne:
                Week1 = "一";
                Week2 = "二";
                Week3 = "三";
                Week4 = "四";
                Week5 = "五";
                Week6 = "六";
                Week7 = "日";
                week -= 1;
                break;
        }
        week %= 7;
        for (int a = 0; a < week; a++)
        {
            Days.Add(new());
        }
        for (int a = 0; a < dt; a++)
        {
            Days.Add(new(year, month, a + 1, color));
        }
    }
}
