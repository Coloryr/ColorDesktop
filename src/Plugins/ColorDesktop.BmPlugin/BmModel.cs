using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.BmPlugin;

public partial class BmModel : ObservableObject
{
    public ObservableCollection<BmItemModel> BmItems { get; init; } = [];

    [ObservableProperty]
    private bool _isOver;
    [ObservableProperty]
    private bool _isUpdate;
    [ObservableProperty]
    private bool _isEmpty;

    [ObservableProperty]
    private DayOfWeek _week;

    private List<BmObj> _bm;

    partial void OnWeekChanged(DayOfWeek value)
    {
        if (_bm == null || _bm.Count == 0)
        {
            IsEmpty = true;
            return;
        }

        BmItems.Clear();
        var data = _bm[((int)value + 6) % 7].Items;
        foreach (var item in data)
        {
            BmItems.Add(new(item));
        }
        IsEmpty = false;
    }

    public void Init()
    {
        var date = DateTime.Now;

        Week = date.DayOfWeek;
    }

    public void Tick()
    {
        var now = DateTime.Now;
        if (now.DayOfWeek != Week)
        {
            Week = now.DayOfWeek;
            _ = Load();
        }
    }

    [RelayCommand]
    public async Task Load()
    {
        IsUpdate = true;
        try
        {
            var bm = await BmApi.GetBm();
            if (bm == null)
            {
                IsEmpty = true;
                IsUpdate = false;
                return;
            }

            _bm = bm;
            OnWeekChanged(Week);
        }
        catch
        {

        }
        IsUpdate = false;
    }
}
