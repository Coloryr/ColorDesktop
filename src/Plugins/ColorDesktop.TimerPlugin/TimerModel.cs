using System.Collections.ObjectModel;
using ColorDesktop.Api.Objs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.TimerPlugin;

public partial class TimerModel : ObservableObject
{
    public const string NameTop = "Top";

    private readonly InstanceDataObj _obj;
    private readonly TimerInstanceObj _config;

    public int MaxHeight => _config.MaxHeight;

    public ObservableCollection<TimerItemModel> Instances { get; init; } = [];

    public TimerModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = TimerPlugin.GetConfig(obj);

        _config.Timers ??= [];

        foreach (var item in _config.Timers)
        {
            Instances.Add(new TimerItemModel(this, item));
        }
    }

    [RelayCommand]
    public void AddDownTimer()
    {
        Instances.Insert(0, new TimerItemModel(this, TimerType.Down));
        OnPropertyChanged(NameTop);
    }

    [RelayCommand]
    public void AddUpTimer()
    {
        Instances.Insert(0, new TimerItemModel(this, TimerType.Up));
        OnPropertyChanged(NameTop);
    }

    public void Save(TimerItemObj obj)
    {
        _config.Timers.Remove(obj);
        _config.Timers.Add(obj);

        TimerPlugin.SaveConfig(_obj, _config);
    }

    public void Tick()
    {
        foreach (var item in Instances)
        {
            item.Tick();
        }

        Sort();
    }

    private void Sort()
    {

    }

    public void Stop()
    {
        foreach (var item in Instances)
        {
            item.Stop();
        }

        Instances.Clear();
    }
}
