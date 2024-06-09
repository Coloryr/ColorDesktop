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
        foreach (var item in InstanceMan.Instances)
        {
            Instances.Add(new InstanceItemModel(this, item.Value));
        }
    }

    public void LoadInstanceCount()
    {
        AllInstance = InstanceMan.Instances.Count;
        int count = 0;
        foreach (var item in ConfigHelper.Config.EnableInstance)
        {
            if (InstanceMan.Instances.ContainsKey(item))
            {
                count++;
            }
        }
        EnableInstance = count;
        ErrorInstance = InstanceMan.LoadError.Count;
        FailInstance = InstanceMan.LoadFail.Count;
    }
}
