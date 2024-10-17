using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.MonitorPlugin;

public partial class MonitorModel : ObservableObject
{
    public const string PanelName = "Panel";

    public PanelType PanelType { get; set; }

    [ObservableProperty]
    public double _width;
    [ObservableProperty]
    public double _height;

    [ObservableProperty]
    private bool _init = true;

    public ObservableCollection<MonitorItemModel> Items { get; init; } = [];

    public void Tick()
    {
        foreach (var item in Items)
        {
            item.Update();
        }
    }

    public async void Update(MonitorInstanceObj obj)
    {
        await MonitorPlugin.WaitInit();

        Init = false;

        PanelType = obj.PanelType;

        Items.Clear();
        foreach (var item in obj.Items)
        {
            Items.Add(new(item));
        }

        if (obj.AutoSize)
        {
            Width = Height = double.NaN;
        }
        else
        {
            Width = obj.Width;
            Height = obj.Height;
        }

        OnPropertyChanged(PanelName);
    }
}
