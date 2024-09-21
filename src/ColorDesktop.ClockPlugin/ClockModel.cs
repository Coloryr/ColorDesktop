using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.ClockPlugin;

public partial class ClockModel : ObservableObject
{
    [ObservableProperty]
    private string _hour;
    [ObservableProperty]
    private string _minute;
    [ObservableProperty]
    private string _second;

    [ObservableProperty]
    private int _hourSize = 50;
    [ObservableProperty]
    private int _minuteSize = 50;
    [ObservableProperty]
    private int _secondSize = 50;
    [ObservableProperty]
    private int _colonSize = 50;

    [ObservableProperty]
    private IBrush _hourColor = Brushes.Black;
    [ObservableProperty]
    private IBrush _minuteColor = Brushes.Black;
    [ObservableProperty]
    private IBrush _secondColor = Brushes.Black;
    [ObservableProperty]
    private IBrush _colonColor = Brushes.Black;

    [ObservableProperty]
    private IBrush _backGround = Brushes.Transparent;

    [ObservableProperty]
    private FontFamily _font;

    [ObservableProperty]
    private double _blink = 1;

    [ObservableProperty]
    private bool _displaySecond;

    private int _last = -1;
    private bool _isBlink;
    private bool _lastBlink;

    public void Update(ClockInstanceObj obj)
    {
        if (obj.UseFont)
        {
            if (!string.IsNullOrWhiteSpace(obj.FontName)
            && FontManager.Current.SystemFonts.Any(a => a.Name == obj.FontName)
            && SkiaSharp.SKFontManager.Default.MatchFamily(obj.FontName) is { } font)
            {
                Font = new(font.FamilyName);
            }
            else
            {
                Font = new(FontFamily.DefaultFontFamilyName);
            }
        }

        DisplaySecond = obj.DisplaySecond;
        _isBlink = obj.Blink;
        if (obj.CustomColor)
        {
            HourColor = Brush.Parse(obj.HourColor);
            MinuteColor = Brush.Parse(obj.MinuteColor);
            SecondColor = Brush.Parse(obj.SecondColor);
            ColonColor = Brush.Parse(obj.ColonColor);
        }
        else
        {
            var color = Brush.Parse(obj.Color);
            HourColor = color;
            MinuteColor = color;
            SecondColor = color;
            ColonColor = color;
        }

        if (obj.CustomSize)
        {
            HourSize = obj.HourSize;
            MinuteSize = obj.MinuteSize;
            SecondSize = obj.SecondSize;
            ColonSize = obj.ColonSize;
        }
        else
        {
            HourSize = obj.Size;
            MinuteSize = obj.Size;
            SecondSize = obj.Size;
            ColonSize = obj.Size;
        }

        BackGround = Brush.Parse(obj.BackGround);
    }

    public void Tick()
    {
        var time = ClockPlugin.Config.Ntp ? NtpClient.Date : DateTime.Now;
        Hour = string.Format("{0:D2}", time.Hour);
        Minute = string.Format("{0:D2}", time.Minute);
        Second = string.Format("{0:D2}", time.Second);
        if (_last != time.Second)
        {
            _last = time.Second;
            if (_isBlink)
            {
                if (_lastBlink)
                {
                    Blink = 1.0;
                }
                else
                {
                    Blink = 0;
                }
                _lastBlink = !_lastBlink;
            }
        }
    }
}
