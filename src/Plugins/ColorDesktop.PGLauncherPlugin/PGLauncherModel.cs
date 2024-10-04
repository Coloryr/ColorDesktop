using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.PGLauncherPlugin;

public partial class PGLauncherModel : ObservableObject
{
    public const string PanelName = "Panel";

    public ObservableCollection<PGItemModel> Items { get; init; } = [];

    public PanelType PanelType { get; set; }

    [ObservableProperty]
    public int _width;
    [ObservableProperty]
    public int _height;

    public void Update(PGLauncherInstanceObj obj)
    {
        PanelType = obj.PanelType;

        Items.Clear();
        foreach (var item in obj.Items)
        {
            Items.Add(new(item));
        }

        Width = obj.Width;
        Height = obj.Height;

        OnPropertyChanged(PanelName);
    }
}
