using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AvaloniaEdit.Utils;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Manager;
using ColorDesktop.Launcher.UI.Models.Dialog;
using ColorDesktop.Launcher.UI.Models.Items;
using ColorDesktop.Launcher.UI.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher.UI.Models.Main;

public partial class MainViewModel
{
    public string[] TypeInstanceNames { get; init; } = LangHelper.GetInstanceTypeLang();

    public ObservableCollection<InstanceItemModel> Instances { get; init; } = [];

    private readonly List<InstanceItemModel> _instances = [];

    [ObservableProperty]
    private int _allInstance;
    [ObservableProperty]
    private int _enableInstance;
    [ObservableProperty]
    private int _errorInstance;
    [ObservableProperty]
    private int _failInstance;

    [ObservableProperty]
    private int _selectInstanceType;

    [ObservableProperty]
    private string _selectInstanceName;

    partial void OnSelectInstanceNameChanged(string value)
    {
        LoadInstanceList();
    }

    [RelayCommand]
    public void LoadInstanceData()
    {
        _instances.Clear();

        LoadInstanceCount();
        foreach (var item in InstanceManager.Instances)
        {
            _instances.Add(new InstanceItemModel(this, item.Value));
        };
        LoadInstanceList();
    }

    private void LoadInstanceList()
    {
        Instances.Clear();
        string name = SelectInstanceName;
        if (string.IsNullOrWhiteSpace(name))
        {
            Instances.AddRange(_instances);
        }
        else
        {
            if (SelectInstanceType == 0)
            {
                Instances.AddRange(_instances.Where(item => item.Plugin.Contains(name)));
            }
            else if (SelectInstanceType == 1)
            {
                Instances.AddRange(_instances.Where(item => item.Nick.Contains(name)));
            }
            else if (SelectInstanceType == 2)
            {
                Instances.AddRange(_instances.Where(item => item.UUID.Contains(name)));
            }
        }

        foreach (var item in Instances)
        {
            item.Update();
        }
    }

    private void LoadInstanceCount()
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
        ErrorInstance = InstanceManager.InstanceStates.Count(item => item.Value == InstanceState.LoadError);
        FailInstance = InstanceManager.InstanceStates.Count(item => item.Value == InstanceState.LoadFail);
    }

    public async void Delete(InstanceItemModel model)
    {
        var res = await DialogHost.Show(new ChoiseModel()
        {
            Text = string.Format(LangApi.GetLang("MainWindow.Info5"), model.Nick)
        }, MainWindow.DialogHostName);

        if (res is true)
        {
            InstanceManager.Delete(model.Plugin, model.UUID);
            LoadInstanceData();
        }
    }
}
