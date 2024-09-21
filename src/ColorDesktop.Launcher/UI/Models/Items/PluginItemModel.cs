using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Manager;
using ColorDesktop.Launcher.UI.Models.Dialog;
using ColorDesktop.Launcher.UI.Models.Main;
using ColorDesktop.Launcher.UI.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher.UI.Models.Items;

public partial class PluginItemModel : ObservableObject
{
    private readonly PluginDataObj _obj;
    private readonly MainViewModel _model;

    [ObservableProperty]
    private bool _enable;

    [ObservableProperty]
    private bool _loadFail;
    [ObservableProperty]
    private bool _enableFail;

    public string ID => _obj.ID;
    public string Name => _obj.Name;
    public string Describe => _obj.Describe;
    public string Auther => _obj.Auther;
    public string Version => _obj.Version;
    public Task<Bitmap?> Image => GetImage();

    private bool _edit;
    private bool _work;

    public bool HaveSetting { get; init; }

    public PluginItemModel(MainViewModel model, PluginDataObj obj)
    {
        _obj = obj;
        _model = model;

        _enable = PluginManager.IsEnable(obj.ID);
        _loadFail = PluginManager.IsFail(obj.ID);
        _enableFail = PluginManager.IsEnableFail(obj.ID);

        HaveSetting = PluginManager.HavePluginSetting(obj.ID);
    }

    async partial void OnEnableChanged(bool value)
    {
        if (_edit || _work)
        {
            return;
        }

        _work = true;
        if (value)
        {
            var res = await DialogHost.Show(new ChoiseModel()
            {
                Text = "是否要启用该插件"
            }, MainWindow.DialogHostName);

            if (res is not true)
            {
                _edit = true;
                Enable = false;
                _edit = false;
                _work = false;
                return;
            }

            PluginManager.EnablePlugin(_obj.ID);
        }
        else
        {
            var res = await DialogHost.Show(new ChoiseModel()
            {
                Text = "是否要禁用该插件"
            }, MainWindow.DialogHostName);

            if (res is not true)
            {
                _edit = true;
                Enable = true;
                _edit = false;
                _work = false;
                return;
            }

            PluginManager.DisablePlugin(_obj.ID);
        }

        Enable = PluginManager.IsEnable(_obj.ID);
        LoadFail = PluginManager.IsFail(_obj.ID);
        EnableFail = PluginManager.IsEnableFail(_obj.ID);

        _model.LoadCount();

        _work = false;
    }

    [RelayCommand]
    public void OpenSetting()
    {
        if (!Enable || LoadFail || EnableFail)
        {
            return;
        }
        PluginManager.OpenSetting(_obj);
    }

    [RelayCommand]
    public void CreateInstance()
    {
        if (!Enable || LoadFail || EnableFail)
        {
            return;
        }
        InstanceManager.CreateInstance(_obj);
    }

    public Task<Bitmap?> GetImage()
    {
        return Task.Run(() =>
        {
            if (PluginManager.PluginAssemblys.TryGetValue(_obj.ID, out var dll))
            {
                return dll.Plugin.GetIcon();
            }

            return null;
        });
    }

    public void Update()
    {
        Enable = PluginManager.IsEnable(_obj.ID);
        LoadFail = PluginManager.IsFail(_obj.ID);
        EnableFail = PluginManager.IsEnableFail(_obj.ID);
    }
}
