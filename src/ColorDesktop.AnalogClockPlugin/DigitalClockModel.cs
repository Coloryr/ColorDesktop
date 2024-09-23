using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.ClockPlugin;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.AnalogClockPlugin;

public partial class DigitalClockModel : ObservableObject
{
    public DigitalModel HourA { get; init; } = new();
    public DigitalModel HourB { get; init; } = new();
    public DigitalModel MinuteA { get; init; } = new();
    public DigitalModel MinuteB { get; init; } = new();
    public DigitalModel SecondA { get; init; } = new();
    public DigitalModel SecondB { get; init; } = new();

    public PointModel Point { get; init; } = new();

    [ObservableProperty]
    private bool _displaySecond;

    private bool _blink;
    private bool _enable;
    private int _sec;

    public void Tick()
    {
        var time = ClockPlugin.ClockPlugin.Config.Ntp ? NtpClient.Date : DateTime.Now;

        HourA.SetValue(time.Hour / 10);
        HourB.SetValue(time.Hour % 10);

        MinuteA.SetValue(time.Minute / 10);
        MinuteB.SetValue(time.Minute % 10);

        SecondA.SetValue(time.Second / 10);
        SecondB.SetValue(time.Second % 10);

        if (_sec != time.Second)
        {
            _sec = time.Second;
            _blink = !_blink;
            if (_enable)
            {
                Point.SetState(_blink);
            }
            else
            {
                Point.SetState(true);
            }
        }
    }

    public void Update(AnalogClockConfigObj obj)
    {
        HourA.Update(obj);
        HourB.Update(obj);
        MinuteA.Update(obj);
        MinuteB.Update(obj);
        SecondA.Update(obj);
        SecondB.Update(obj);

        Point.Update(obj);

        DisplaySecond = obj.DisplaySecond;

        _enable = obj.Blink;
    }
}
