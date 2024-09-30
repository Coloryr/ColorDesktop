using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using ColorDesktop.ClockPlugin;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lunar;

namespace ColorDesktop.CalendarPlugin;

public partial class CalendarModel : ObservableObject
{
    public const string LoadMonthName = "LoadMonth";

    [ObservableProperty]
    private IBrush _backColor;
    [ObservableProperty]
    private IBrush _textColor;

    [ObservableProperty]
    private string _date;
    [ObservableProperty]
    private string _lDate;
    [ObservableProperty]
    private string _tian;
    [ObservableProperty]
    private string _week;
    [ObservableProperty]
    private string _wuhang;
    [ObservableProperty]
    private string _jieqi;
    [ObservableProperty]
    private string _chongsha;
    [ObservableProperty]
    private string _pengzu;
    [ObservableProperty]
    private string _yi;
    [ObservableProperty]
    private string _ji;

    [ObservableProperty]
    private int _nowYear;
    [ObservableProperty]
    private int _nowMouth;

    [ObservableProperty]
    private bool _isOpenInfo;
    [ObservableProperty]
    private bool _isOpenDate;
    [ObservableProperty]
    private bool _showButton;
    [ObservableProperty]
    private bool _haveJieqi;

    public WeekStart WeekStart;

    private DateTime _last;

    public void Tick()
    {
        var time = ClockPlugin.ClockPlugin.Config.Ntp ? NtpClient.Date : DateTime.Now;

        if (_last.DayOfYear == time.DayOfYear)
        {
            return;
        }

        _last = time;

        Date = time.ToString("D");

        var solar = new Solar(time);

        var lunar = solar.Lunar;

        LDate = lunar.MonthInChinese + "月" + lunar.DayInChinese;
        Week = solar.WeekInChinese;
        Tian = lunar.YearInGanZhi + lunar.YearShengXiao + "年 " + lunar.MonthInGanZhi + "月 " + lunar.DayInGanZhi + "日";
        Wuhang = lunar.DayNaYin;
        Chongsha = "冲" + lunar.DayChongDesc + " 煞" + lunar.DaySha;
        Pengzu = lunar.PengZuGan + Environment.NewLine + lunar.PengZuZhi;
        
        if (NowYear == 0)
        {
            NowYear = _last.Year;
        }
        OnPropertyChanged(LoadMonthName);
        if (NowMouth == 0)
        {
            NowMouth = _last.Month;
        }

        foreach (var item in lunar.DayYi)
        {
            Yi += item + " ";
        }

        foreach (var item in lunar.DayJi)
        {
            Ji += item + " ";
        }

        var jie = lunar.CurrentJieQi;
        if (jie != null)
        {
            HaveJieqi = true;
            Jieqi = jie.Name;
        }
        else
        {
            HaveJieqi = false;
        }
    }

    [RelayCommand]
    public void OpenInfo()
    {
        IsOpenInfo = !IsOpenInfo;
    }

    [RelayCommand]
    public void OpenDate()
    {
        IsOpenDate = !IsOpenDate;
    }

    [RelayCommand]
    public void LastMonth()
    {
        if (NowMouth == 1)
        {
            NowYear--;
            OnPropertyChanged(LoadMonthName);
            NowMouth = 12;
        }
        else
        {
            NowMouth--;
        }
    }

    [RelayCommand]
    public void NextMonth()
    {
        if (NowMouth == 12)
        {
            NowYear++;
            OnPropertyChanged(LoadMonthName);
            NowMouth = 1;
        }
        else
        {
            NowMouth++;
        }
    }

    public void Update(CalendarInstanceObj config)
    {
        WeekStart = config.WeekStart;
        BackColor = Brush.Parse(config.BackColor);
        TextColor = Brush.Parse(config.TextColor);
        _last = DateTime.MinValue;
    }
}
