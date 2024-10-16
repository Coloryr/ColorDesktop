using Avalonia.Media;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;
using Lunar;

namespace ColorDesktop.CalendarPlugin.Skin2;

public partial class DayModel : ObservableObject
{
    private static readonly IBrush s_brush1 = Brush.Parse("#408af3");
    private static readonly IBrush s_brush2 = Brush.Parse("#404040");

    public int Day { get; init; }
    public string Lunar { get; init; }
    public string Tip { get; init; }
    public IBrush Color { get; init; }
    public IBrush ColorNum { get; init; }
    public IBrush Back { get; init; }

    private readonly Solar _solar;

    public DayModel(int year, int month, int day)
    {
        Day = day;
        _solar = new(year, month, day);

        var tip = $"{year}/{month}/{day}";

        var temp = _solar.Lunar;
        tip += temp.MonthInChinese + LangApi.GetLang("CalendarPluginControl.Text9")
            + temp.DayInChinese;

        if (!string.IsNullOrWhiteSpace(temp.JieQi))
        {
            tip += " " + temp.JieQi;

            Lunar = temp.JieQi;

            Color = Brushes.Red;
        }
        else if (temp.Day == 1)
        {
            Lunar = temp.MonthInChinese + LangApi.GetLang("CalendarPluginControl.Text9");

            Color = s_brush1;
        }
        else
        {
            Lunar = temp.DayInChinese;

            Color = s_brush2;
        }
        var now = DateTime.Now;
        if (year == now.Year && month == now.Month && day == now.Day)
        {
            Back = s_brush1;
            Color = Brushes.White;
            ColorNum = Brushes.White;
        }
        else
        {
            ColorNum = Brushes.Black;
        }

        Tip = tip;
    }

    public DayModel()
    {

    }
}
