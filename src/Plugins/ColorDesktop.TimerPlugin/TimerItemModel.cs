using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.TimerPlugin;

public partial class TimerItemModel : ObservableObject
{
    private readonly TimerModel _top;
    private readonly TimerItemObj _obj;

    [ObservableProperty]
    private bool _isEdit;

    [ObservableProperty]
    private bool _isDown;

    [ObservableProperty]
    private int? _hour = 0;
    [ObservableProperty]
    private int? _minute = 0;
    [ObservableProperty]
    private int? _second = 0;

    [ObservableProperty]
    private string _time;

    public TimerItemModel(TimerModel top, TimerItemObj obj)
    {
        _top = top;
        _obj = obj;
    }

    public TimerItemModel(TimerModel top, TimerType type)
    {
        _top = top;
        _obj = new TimerItemObj()
        {
            Type = type
        };
        _isEdit = true;
    }

    [RelayCommand]
    public void TimeHourUp()
    {
        if (Hour < 99)
        {
            Hour++;
        }
    }

    [RelayCommand]
    public void TimeHourDown()
    {
        if (Hour > 0)
        {
            Hour--;
        }
    }

    [RelayCommand]
    public void TimeMinuteUp()
    {
        if (Minute < 59)
        {
            Minute++;
        }
        else if (Minute == 59)
        {
            TimeHourUp();
            Minute = 0;
        }
    }

    [RelayCommand]
    public void TimeMinuteDown()
    {
        if (Minute > 0)
        {
            Minute--;
        }
        else if (Minute == 0)
        {
            TimeHourDown();
            Minute = 59;
        }
    }

    [RelayCommand]
    public void TimeSecondUp()
    {
        if (Second < 59)
        {
            Second++;
        }
        else if (Minute == 59)
        {
            TimeMinuteUp();
            Second = 0;
        }
    }

    [RelayCommand]
    public void TimeSecondDown()
    {
        if (Second > 0)
        {
            Second--;
        }
        else if (Second == 0)
        {
            TimeMinuteDown();
            Second = 59;
        }
    }

    [RelayCommand]
    public void Done()
    {
        IsEdit = false;
    }

    [RelayCommand]
    public void Cancel()
    {
        IsEdit = true;
    }

    public void Tick()
    {
        if (IsEdit)
        {
            return;
        }
    }

    public void Stop()
    {

    }
}
