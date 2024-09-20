using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaEdit.Utils;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Manager;
using ColorDesktop.Launcher.UI.Models.Items;
using ColorDesktop.Launcher.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.Launcher.UI.Models.Main;

public partial class MainViewModel
{
    public string[] TypePluginNames { get; init; } = LangHelper.GetPluginTypeLang();

    public ObservableCollection<PluginItemModel> Plugins { get; init; } = [];

    private readonly List<PluginItemModel> _plugins = [];

    [ObservableProperty]
    private int _allPlugin;
    [ObservableProperty]
    private int _enablePlugin;
    [ObservableProperty]
    private int _errorPlugin;
    [ObservableProperty]
    private int _failPlugin;

    [ObservableProperty]
    private int _selectPluginType;

    [ObservableProperty]
    private string _selectPluginName;

    partial void OnSelectPluginNameChanged(string value)
    {
        LoadPluginList();
    }

    [RelayCommand]
    public void LoadPluginData()
    {
        _plugins.Clear();

        LoadCount();
        foreach (var item in PluginManager.Plugins)
        {
            _plugins.Add(new PluginItemModel(this, item.Value));
        }

        LoadPluginList();
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
        FailPlugin = PluginManager.EnableError.Count;
    }

    private void LoadPluginList()
    {
        Plugins.Clear();

        string name = SelectPluginName;
        if (string.IsNullOrWhiteSpace(name))
        {
            Plugins.AddRange(_plugins);
        }
        else
        {
            if (SelectPluginType == 0)
            {
                Plugins.AddRange(_plugins.Where(item=>item.ID.Contains(name)));
            }
            else if (SelectPluginType == 1)
            {
                Plugins.AddRange(_plugins.Where(item => item.Name.Contains(name)));
            }
            else if (SelectPluginType == 2)
            {
                Plugins.AddRange(_plugins.Where(item => item.Auther.Contains(name)));
            }
            else if (SelectPluginType == 3)
            {
                Plugins.AddRange(_plugins.Where(item => item.Describe.Contains(name)));
            }
        }
    }
}
