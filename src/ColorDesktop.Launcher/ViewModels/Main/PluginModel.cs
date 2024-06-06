using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Launcher.Utils;
using ColorDesktop.Launcher.ViewModels.Items;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.Launcher.ViewModels.Main;

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
        foreach (var item in PluginMan.Plugins)
        {
            Plugins.Add(new PluginItemModel(this, item.Value));
        }
    }

    public void LoadCount()
    {
        AllPlugin = PluginMan.Plugins.Count;
        int count = 0;
        foreach (var item in App.Config.EnablePlugin)
        {
            if (PluginMan.Plugins.ContainsKey(item))
            {
                count++;
            }
        }
        ErrorPlugin = PluginMan.LoadError.Count;
        FailPlugin = PluginMan.LoadFail.Count;
    }
}
