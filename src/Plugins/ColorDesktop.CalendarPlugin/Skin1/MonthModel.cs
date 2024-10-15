using System.Collections.ObjectModel;
using Avalonia.Media;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.CalendarPlugin.Skin1;

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
                Week1 = LangApi.GetLang("CalendarSKin1.Text1");
                Week2 = LangApi.GetLang("CalendarSKin1.Text2");
                Week3 = LangApi.GetLang("CalendarSKin1.Text3");
                Week4 = LangApi.GetLang("CalendarSKin1.Text4");
                Week5 = LangApi.GetLang("CalendarSKin1.Text5");
                Week6 = LangApi.GetLang("CalendarSKin1.Text6");
                Week7 = LangApi.GetLang("CalendarSKin1.Text7");
                break;
            case WeekStart.DaySat:
                Week1 = LangApi.GetLang("CalendarSKin1.Text7");
                Week2 = LangApi.GetLang("CalendarSKin1.Text1");
                Week3 = LangApi.GetLang("CalendarSKin1.Text2");
                Week4 = LangApi.GetLang("CalendarSKin1.Text3");
                Week5 = LangApi.GetLang("CalendarSKin1.Text4");
                Week6 = LangApi.GetLang("CalendarSKin1.Text5");
                Week7 = LangApi.GetLang("CalendarSKin1.Text6");
                week += 1;
                break;
            case WeekStart.DayOne:
                Week1 = LangApi.GetLang("CalendarSKin1.Text2");
                Week2 = LangApi.GetLang("CalendarSKin1.Text3");
                Week3 = LangApi.GetLang("CalendarSKin1.Text4");
                Week4 = LangApi.GetLang("CalendarSKin1.Text5");
                Week5 = LangApi.GetLang("CalendarSKin1.Text6");
                Week6 = LangApi.GetLang("CalendarSKin1.Text7");
                Week7 = LangApi.GetLang("CalendarSKin1.Text1");
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
