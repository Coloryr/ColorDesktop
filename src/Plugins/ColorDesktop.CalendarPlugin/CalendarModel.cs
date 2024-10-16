using Avalonia.Media;
using ColorDesktop.Api;
using ColorDesktop.ClockPlugin;
using ColorDesktop.CoreLib;
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
    private string _history;

    [ObservableProperty]
    private int _nowYear;
    [ObservableProperty]
    private int _nowMouth;

    [ObservableProperty]
    private bool _isOpenInfo;
    [ObservableProperty]
    private bool _isOpenDate;
    [ObservableProperty]
    private bool _isOpenHistory;
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

        LDate = lunar.MonthInChinese + LangApi.GetLang("CalendarPluginControl.Text9") + lunar.DayInChinese;
        Week = solar.WeekInChinese;
        Tian = lunar.YearInGanZhi + lunar.YearShengXiao 
            + LangApi.GetLang("CalendarPluginControl.Text8") 
            + " " + lunar.MonthInGanZhi + LangApi.GetLang("CalendarPluginControl.Text9") 
            + " " + lunar.DayInGanZhi + LangApi.GetLang("CalendarPluginControl.Text14");
        Wuhang = lunar.DayNaYin;
        Chongsha = LangApi.GetLang("CalendarPluginControl.Text15") + lunar.DayChongDesc 
            + " " + LangApi.GetLang("CalendarPluginControl.Text16") + lunar.DaySha;
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

    [RelayCommand]
    public void BackNow()
    {
        if (NowYear != _last.Year)
        {
            NowYear = _last.Year;
            OnPropertyChanged(LoadMonthName);
        }
        NowMouth = _last.Month;
    }

    [RelayCommand]
    public async Task OpenHistory()
    {
        IsOpenHistory = !IsOpenHistory;
        if (IsOpenHistory)
        {
            try
            {
                var data = await HttpUtils.Client.GetStringAsync("https://xiaoapi.cn/API/lssdjt.php");
                History = data.Trim();
            }
            catch
            {
                History = LangApi.GetLang("CalendarPluginControl.Error1");
            }
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
