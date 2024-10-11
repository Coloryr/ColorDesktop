using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.UI.Models.Items;
using ColorDesktop.Launcher.UI.Windows;
using ColorDesktop.Launcher.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher.UI.Models.Dialog;

public partial class PluginSourceModel : ObservableObject
{
    public const string GoEditName = "GoEdit";

    public ObservableCollection<PluginSourceItemModel> Sources { get; init; } = [];

    [ObservableProperty]
    private PluginSourceItemModel? _select;

    public PluginSourceModel()
    {
        var list = ConfigHelper.Config.PluginSource;
        foreach (var item in list)
        {
            Sources.Add(new(this)
            {
                Enable = item.Enable,
                Url = item.Url
            });
        }
    }

    [RelayCommand]
    public void Done()
    {
        DialogHost.Close(MainWindow.DialogHostName);
    }

    [RelayCommand]
    public void AddSource()
    {
        var item = new PluginSourceItemModel(this)
        {
            Enable = true
        };
        Sources.Add(item);
        Select = item;
        OnPropertyChanged(GoEditName);
    }

    public void Remove(PluginSourceItemModel model, string url)
    {
        Sources.Remove(model);
        ConfigHelper.RemoveSource(url);
    }
}
