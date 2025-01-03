﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
    public record GroupItemModel
    { 
        public string Name { get; set; }
        public string UUID { get; set; }
    }

    public string[] TypeInstanceNames { get; init; } = LangHelper.GetInstanceTypeLang();

    public ObservableCollection<InstanceItemModel> Instances { get; init; } = [];

    public ObservableCollection<GroupItemModel> InstanceGroups { get; init; } = [];

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
    private int _groupIndex;

    [ObservableProperty]
    private string _selectInstanceName;

    [ObservableProperty]
    private bool _isDefaultGroup;

    private bool _instanceGroupLoad;

    partial void OnSelectInstanceNameChanged(string value)
    {
        LoadInstanceList();
    }

    partial void OnGroupIndexChanged(int value)
    {
        if (_instanceGroupLoad)
        {
            return;
        }

        InstanceManager.SwitchGroup(InstanceGroups[GroupIndex].UUID);

        LoadInstances();
        LoadInstanceList();

        IsDefaultGroup = GroupIndex == 0;
    }

    [RelayCommand]
    public async Task CreateGroup()
    {
        var obj = new InputModel()
        { 
            Text = LangApi.GetLang("MainWindow.Text69")
        };
        var res = await DialogHost.Show(obj, MainWindow.DialogHostName);
        if (res is true)
        {
            if (string.IsNullOrWhiteSpace(obj.Input))
            {
                return;
            }

            InstanceManager.CreateGroup(obj.Input);
            LoadInstanceData();
        }
    }

    [RelayCommand]
    public async Task DeleteGroup()
    {
        if (GroupIndex == 0)
        {
            return;
        }
        var item = InstanceGroups[GroupIndex];
        var obj = new ChoiseModel()
        {
            Text = string.Format(LangApi.GetLang("MainWindow.Text70"), item.Name)
        };
        var res = await DialogHost.Show(obj, MainWindow.DialogHostName);
        if (res is true)
        {
            InstanceManager.DeleteGroupUUID(item.UUID);
            LoadInstanceData();
        }
    }

    [RelayCommand]
    public async Task ImportInstance()
    {
        if (GroupIndex == 0)
        {
            return;
        }
        var obj = new ChoiseInstanceModel(InstanceGroups[GroupIndex].UUID);
        var res = await DialogHost.Show(obj);
        if (res is not true)
        {
            return;
        }
        var list = new List<string>();
        foreach (var item in obj.Items)
        {
            if (item.IsCheck)
            {
                list.Add(item.UUID);
            }
        }
        InstanceManager.GroupImportInstance(InstanceGroups[GroupIndex].UUID, list);
        LoadInstances();
        LoadInstanceList();
    }

    [RelayCommand]
    public void LoadInstanceData()
    {
        LoadGroup();
        LoadInstanceCount();
        LoadInstances();
        LoadInstanceList();
    }

    private void LoadGroup()
    {
        _instanceGroupLoad = true;

        InstanceGroups.Clear();
        InstanceGroups.Add(new() { Name = LangApi.GetLang("MainWindow.Text66") });

        foreach (var item in InstanceManager.Groups)
        {
            InstanceGroups.Add(new() { UUID = item.Key, Name = item.Value.Name });
        }

        GroupIndex = 0;
        if (!string.IsNullOrWhiteSpace(ConfigHelper.Config.Group))
        {
            for (int a = 1; a < InstanceGroups.Count; a++)
            {
                var item = InstanceGroups[a];
                if (item.UUID == ConfigHelper.Config.Group)
                {
                    GroupIndex = a;
                    break;
                }
            }
        }
        IsDefaultGroup = GroupIndex == 0;

        _instanceGroupLoad = false;
    }

    private void LoadInstances()
    {
        _instances.Clear();
        if (GroupIndex == 0)
        {
            foreach (var item in InstanceManager.Instances)
            {
                _instances.Add(new InstanceItemModel(this, item.Value));
            };
        }
        else
        {
            var group = InstanceManager.Groups[InstanceGroups[GroupIndex].UUID];
            foreach (var item in InstanceManager.Instances)
            {
                if (group.Instances.Contains(item.Key))
                {
                    _instances.Add(new InstanceItemModel(this, item.Value));
                }
            };
        }
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
        if (GroupIndex == 0)
        {
            foreach (var item in ConfigHelper.Config.EnableInstance)
            {
                if (InstanceManager.Instances.ContainsKey(item))
                {
                    count++;
                }
            }
        }
        else
        {
            var group = InstanceManager.Groups[InstanceGroups[GroupIndex].UUID];
            foreach (var item in group.Enables)
            {
                if (InstanceManager.Instances.ContainsKey(item))
                {
                    count++;
                }
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
