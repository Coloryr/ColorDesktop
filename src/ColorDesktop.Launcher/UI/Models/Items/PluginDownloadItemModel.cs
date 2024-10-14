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

public partial class PluginDownloadItemModel : ObservableObject
{
    [ObservableProperty]
    private bool _have;
    [ObservableProperty]
    private bool _update;

    public string Source => _source;
    public string ID => _obj.ID;
    public string Name => _obj.Name;
    public string Describe => _obj.Describe;
    public string Auther => _obj.Auther;
    public string Version => _obj.Version;
    public Task<Bitmap?> Image => GetImage();

    private readonly MainViewModel _model;
    private readonly PluginDownloadObj.ItemObj _obj;
    private readonly string _source;
    private readonly string _baseurl;

    public PluginDownloadItemModel(MainViewModel model, PluginDownloadObj.ItemObj obj, string source, string baseurl)
    {
        _model = model;
        _obj = obj;
        _have = PluginManager.HavePlugin(obj.ID);
        _update = HaveUpdate();
        _source = source;
        _baseurl = baseurl;
    }

    [RelayCommand]
    public async Task Download()
    {
        var model = new ChoiseModel()
        {
            Text = string.Format(LangApi.GetLang("MainWindow.Info11"), _obj.Name)
        };
        var res = await DialogHost.Show(model, MainWindow.DialogHostName);
        if (res is not true)
        {
            return;
        }

        _ = DialogHost.Show(LangApi.GetLang("MainWindow.Info13"), MainWindow.DialogHostName);
        res = await PluginManager.Download(_obj, _baseurl);
        DialogHost.Close(MainWindow.DialogHostName);
        if (res is not true)
        {
            _ = DialogHost.Show(new ChoiseModel()
            {
                Text = LangApi.GetLang("MainWindow.Info14"),
                HaveCancel = false
            }, MainWindow.DialogHostName);
        }
        else
        {
            _ = DialogHost.Show(new ChoiseModel()
            {
                Text = LangApi.GetLang("MainWindow.Info15"),
                HaveCancel = false
            }, MainWindow.DialogHostName);
        }
    }

    [RelayCommand]
    public async Task Upgrade()
    {
        var model = new ChoiseModel()
        {
            Text = string.Format(LangApi.GetLang("MainWindow.Info16"), _obj.Name, _obj.Version)
        };
        var res = await DialogHost.Show(model, MainWindow.DialogHostName);
        if (res is not true)
        {
            return;
        }

        _ = DialogHost.Show(LangApi.GetLang("MainWindow.Info13"), MainWindow.DialogHostName);
        var dir = PluginManager.DeletePlugin(_obj);
        if (dir == null)
        {
            DialogHost.Close(MainWindow.DialogHostName);
            _ = DialogHost.Show(new ChoiseModel()
            {
                Text = LangApi.GetLang("MainWindow.Info14"),
                HaveCancel = false
            }, MainWindow.DialogHostName);
        }
        res = await PluginManager.Download(_obj, _baseurl, dir);
        DialogHost.Close(MainWindow.DialogHostName);
        if (res is not true)
        {
            _ = DialogHost.Show(new ChoiseModel()
            {
                Text = LangApi.GetLang("MainWindow.Info14"),
                HaveCancel = false
            }, MainWindow.DialogHostName);
        }
        else
        {
            _ = DialogHost.Show(new ChoiseModel()
            {
                Text = LangApi.GetLang("MainWindow.Info15"),
                HaveCancel = false
            }, MainWindow.DialogHostName);
        }
    }

    public Task<Bitmap?> GetImage()
    {
        return Task.Run(async () =>
        {
            if (string.IsNullOrWhiteSpace(_obj.Icon))
            {
                return null;
            }
            string url;
            if (_obj.Icon.StartsWith("http"))
            {
                url = _obj.Icon;
            }
            else
            {
                url = _baseurl + _obj.Url + "/" + _obj.Icon;
            }

            return await TempManager.LoadImage(url);
        });
    }

    private bool HaveUpdate()
    {
        if (!Have)
        {
            return false;
        }
        var version = PluginManager.GetPluginVersion(_obj.ID);
        if (version == null)
        {
            return false;
        }

        return version != _obj.Version;
    }
}
