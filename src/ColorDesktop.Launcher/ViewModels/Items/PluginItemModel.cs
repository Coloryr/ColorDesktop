using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Utils;
using ColorDesktop.Launcher.ViewModels.Main;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.Launcher.ViewModels.Items;

public partial class PluginItemModel : ObservableObject
{
    private readonly PluginDataObj _obj;
    private readonly MainViewModel _model;
    private Bitmap? _bitmap;

    [ObservableProperty]
    private bool _enable;

    [ObservableProperty]
    private bool _fail;

    public string ID => _obj.ID;
    public string Name => _obj.Name;
    public string Describe => _obj.Describe;
    public string Version => _obj.Version;
    public Task<Bitmap?> Image => GetImage();

    public PluginItemModel(MainViewModel model, PluginDataObj obj)
    {
        _obj = obj;
        _model = model;

        _enable = ConfigHelper.Config.EnablePlugin.Contains(obj.ID);
        _fail = PluginMan.IsFail(obj.ID);
    }

    partial void OnEnableChanged(bool value)
    {
        if (value)
        {
            (bool, string?) ok = App.EnablePlugin(_obj.ID);
            if (!ok.Item1)
            {
                Enable = false;
            }
            else
            {
                _model.LoadCount();
            }
        }
        else
        {
            (bool, string?) ok = App.DisablePlugin(_obj.ID);
            if (!ok.Item1)
            {
                Enable = false;
            }
        }
    }

    [RelayCommand]
    public void OpenSetting()
    {
        if (!Enable || Fail)
        {
            return;
        }
        App.OpenSetting(_obj);
    }

    [RelayCommand]
    public void CreateInstance()
    {
        if (!Enable || Fail)
        {
            return;
        }
        App.CreateInstance(_obj);
    }

    public Task<Bitmap?> GetImage()
    {
        return Task.Run(() =>
        {
            var local = Path.GetFullPath(_obj.Local + "/" + _obj.Icon);
            if (File.Exists(local))
            {
                _bitmap = new Bitmap(local);

                return _bitmap;
            }

            return null;
        });
    }
}
