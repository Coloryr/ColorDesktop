using Avalonia.Media;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;
using Lunar;

namespace ColorDesktop.CalendarPlugin.Skin1;

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
            Lunar = temp.MonthInChinese + LangApi.GetLang("CalendarPluginControl.Text9");
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
