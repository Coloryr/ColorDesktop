using System;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Manager;
using ColorDesktop.Launcher.Objs;
using ColorDesktop.Launcher.UI.Models.Dialog;
using ColorDesktop.Launcher.UI.Models.Main;
using ColorDesktop.Launcher.UI.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher.UI.Models.Items;

public partial class PluginItemModel : ObservableObject
{
    [ObservableProperty]
    private bool _enable;

    [ObservableProperty]
    private bool _loadFail;
    [ObservableProperty]
    private bool _enableFail;
    [ObservableProperty]
    private bool _notDep;
    [ObservableProperty]
    private bool _isUnload;

    public bool Core { get; init; } 
    public string ID => _obj.ID;
    public string Name => _obj.Name;
    public string Describe => _obj.Describe;
    public string Auther => _obj.Auther;
    public string Version => _obj.Version;
    public string Tip => MakeTip();
    public Task<Bitmap?> Image => GetImage();

    private bool _edit;
    private bool _work;

    public bool HaveSetting { get; init; }

    private readonly MainViewModel _model;
    private readonly PluginDataObj _obj;

    public PluginItemModel(MainViewModel model, PluginDataObj obj)
    {
        _obj = obj;
        _model = model;

        HaveSetting = PluginManager.HavePluginSetting(obj.ID);
        Core = PluginManager.IsCoreLib(obj.ID);

        _edit = true;
        Update();
        _edit = false;
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

            PluginManager.EnablePlugin(_obj.ID);
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

            PluginManager.DisablePlugin(_obj.ID);
        }

        Update();

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
                var stream = dll.Plugin.GetIcon();
                if (stream != null)
                {
                    var bitmap = new Bitmap(stream);
                    stream.Dispose();
                    return bitmap;
                }

                return null;
            }

            return null;
        });
    }

    public void Update()
    {
        var state = PluginManager.GetPluginState(_obj.ID);
        Enable = PluginManager.IsEnable(_obj.ID);
        LoadFail = state is PluginState.LoadError;
        IsUnload = state == PluginState.Unload;
        EnableFail = state == PluginState.EnableError;
        NotDep = state == PluginState.DepNotFound;
    }

    private string MakeTip()
    {
        var builder = new StringBuilder();
        builder.AppendLine(_obj.Describe)
            .AppendLine()
            .Append(App.Lang("MainWindow.Info10"));
        foreach (var item in _obj.Dependents)
        {
            builder.Append(item.ID).Append(' ');
        }

        return builder.ToString().Trim();
    }
}
