using System;
using System.Collections.ObjectModel;
using System.Linq;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Manager;
using ColorDesktop.Launcher.UI.Models.Items;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.Launcher.UI.Models.Main;

public partial class MainViewModel
{
    public ObservableCollection<InstanceItemModel> Instances { get; init; } = [];

    [ObservableProperty]
    private int _allInstance;
    [ObservableProperty]
    private int _enableInstance;
    [ObservableProperty]
    private int _errorInstance;
    [ObservableProperty]
    private int _failInstance;

    [RelayCommand]
    public void LoadInstanceData()
    {
        Instances.Clear();

        LoadInstanceCount();
        foreach (var item in InstanceManager.Instances)
        {
            Instances.Add(new InstanceItemModel(this, item.Value));
        }
    }

    public void LoadInstanceCount()
    {
        AllInstance = InstanceManager.Instances.Count;
        int count = 0;
        foreach (var item in ConfigHelper.Config.EnableInstance)
        {
            if (InstanceManager.Instances.ContainsKey(item))
            {
                count++;
            }
        }
        EnableInstance = count;
        ErrorInstance = InstanceManager.LoadError.Count;
        FailInstance = InstanceManager.LoadFail.Count;
    }
}
