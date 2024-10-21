using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.BmPlugin;

public partial class BmModel : ObservableObject
{
    public const string BmMoveName = "BmMove";

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

    private int day;

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
            BmItems.Add(CreateModel(item));
        }
        IsEmpty = false;

        OnPropertyChanged(BmMoveName);
    }

    protected virtual BmItemModel CreateModel(BmObj.ItemObj item)
    {
        return new(item);
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

    public void Init()
    {
        var date = DateTime.Now;

        Week = date.DayOfWeek;
    }

    public void Tick()
    {
        var now = DateTime.Now;
        if (now.DayOfYear != day)
        {
            day = now.DayOfYear;
            _ = Load();
        }
    }

    public virtual void Update(BmInstanceObj config)
    {

    }
}
