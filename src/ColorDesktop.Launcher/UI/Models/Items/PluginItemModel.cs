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

public partial class PluginItemModel(MainViewModel model, PluginDataObj obj) : ObservableObject
{
    [ObservableProperty]
    private bool _enable = PluginManager.IsEnable(obj.ID);

    [ObservableProperty]
    private bool _loadFail = PluginManager.IsFail(obj.ID);
    [ObservableProperty]
    private bool _enableFail = PluginManager.IsEnableFail(obj.ID);

    public bool Core { get; init; } = PluginManager.IsCoreLib(obj.ID);
    public string ID => obj.ID;
    public string Name => obj.Name;
    public string Describe => obj.Describe;
    public string Auther => obj.Auther;
    public string Version => obj.Version;
    public Task<Bitmap?> Image => GetImage();

    private bool _edit;
    private bool _work;

    public bool HaveSetting { get; init; } = PluginManager.HavePluginSetting(obj.ID);

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
                Text = App.Lang("MainWindow.Info3")
            }, MainWindow.DialogHostName);

            if (res is not true)
            {
                _edit = true;
                Enable = false;
                _edit = false;
                _work = false;
                return;
            }

            PluginManager.EnablePlugin(obj.ID);
        }
        else
        {
            var res = await DialogHost.Show(new ChoiseModel()
            {
                Text = App.Lang("MainWindow.Info4")
            }, MainWindow.DialogHostName);

            if (res is not true)
            {
                _edit = true;
                Enable = true;
                _edit = false;
                _work = false;
                return;
            }

            PluginManager.DisablePlugin(obj.ID);
        }

        Enable = PluginManager.IsEnable(obj.ID);
        LoadFail = PluginManager.IsFail(obj.ID);
        EnableFail = PluginManager.IsEnableFail(obj.ID);

        model.LoadCount();

        _work = false;
    }

    [RelayCommand]
    public void OpenSetting()
    {
        if (!Enable || LoadFail || EnableFail)
        {
            return;
        }
        PluginManager.OpenSetting(obj);
    }

    [RelayCommand]
    public void CreateInstance()
    {
        if (!Enable || LoadFail || EnableFail)
        {
            return;
        }
        InstanceManager.CreateInstance(obj);
    }

    public Task<Bitmap?> GetImage()
    {
        return Task.Run(() =>
        {
            if (PluginManager.PluginAssemblys.TryGetValue(obj.ID, out var dll))
            {
                return dll.Plugin.GetIcon();
            }

            return null;
        });
    }

    public void Update()
    {
        Enable = PluginManager.IsEnable(obj.ID);
        LoadFail = PluginManager.IsFail(obj.ID);
        EnableFail = PluginManager.IsEnableFail(obj.ID);
    }
}
