using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.AnalogClockPlugin;

public partial class PointerClockModel : ObservableObject
{
    [ObservableProperty]
    private int _size;

    [ObservableProperty]
    private RotateTransform _pointer1 = new();
    [ObservableProperty]
    private RotateTransform _pointer2 = new();
    [ObservableProperty]
    private RotateTransform _pointer3 = new();

    [ObservableProperty]
    private string _top;

    [ObservableProperty]
    private bool _displaySecond;

    private readonly string[] ClockTops = ["/Resource/clock_top.svg", "/Resource/clock_top_1.svg"];

    public void Tick()
    {
        var time = ClockPlugin.ClockPlugin.GetTime();

        var hourAngle = time.Hour % 12 * 30 + (time.Minute * 0.5);
        var minuteAngle = time.Minute * 6 + (time.Second * 0.1);
        var secondAngle = time.Second * 6;

        if (Pointer1.Angle != hourAngle)
        {
            Pointer1.Angle = hourAngle;
        }
        if (Pointer2.Angle != minuteAngle)
        {
            Pointer2.Angle = minuteAngle;
        }
        if (Pointer3.Angle != secondAngle)
        {
            Pointer3.Angle = secondAngle;
        }

        if (time.Hour > 6 && time.Hour < 18)
        {
            Top = ClockTops[0];
        }
        else
        {
            Top = ClockTops[1];
        }
    }

    public void Update(AnalogClockInstanceConfigObj obj)
    {
        Size = obj.Size;
        DisplaySecond = obj.DisplaySecond;
    }
}
