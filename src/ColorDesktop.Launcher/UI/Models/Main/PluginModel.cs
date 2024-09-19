using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Manager;
using ColorDesktop.Launcher.UI.Models.Items;
using ColorDesktop.Launcher.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.Launcher.UI.Models.Main;

public partial class MainViewModel
{
    public ObservableCollection<PluginItemModel> Plugins { get; init; } = [];

    [ObservableProperty]
    private int _allPlugin;
    [ObservableProperty]
    private int _enablePlugin;
    [ObservableProperty]
    private int _errorPlugin;
    [ObservableProperty]
    private int _failPlugin;

    [RelayCommand]
    public void LoadPluginData()
    {
        Plugins.Clear();

        LoadCount();
        foreach (var item in PluginManager.Plugins)
        {
            Plugins.Add(new PluginItemModel(this, item.Value));
        }
    }

    public void LoadCount()
    {
        AllPlugin = PluginManager.Plugins.Count;
        int count = 0;
        foreach (var item in ConfigHelper.Config.EnablePlugin)
        {
            if (PluginManager.Plugins.ContainsKey(item))
            {
                count++;
            }
        }
        EnablePlugin = count;
        ErrorPlugin = PluginManager.LoadError.Count;
        FailPlugin = PluginManager.LoadFail.Count;
    }
}
