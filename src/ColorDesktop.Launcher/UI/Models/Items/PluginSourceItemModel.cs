using System.Threading.Tasks;
using Avalonia.Threading;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Objs;
using ColorDesktop.Launcher.UI.Models.Dialog;
using ColorDesktop.Launcher.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace ColorDesktop.Launcher.UI.Models.Items;

public partial class PluginSourceItemModel : ObservableObject
{
    [ObservableProperty]
    private string? _url;

    [ObservableProperty]
    private bool _enable;
    [ObservableProperty]
    private string _isWork;

    private readonly PluginSourceModel _top;

    public PluginSourceItemModel(PluginSourceModel model)
    {
        _top = model;

        Dispatcher.UIThread.Post(async () =>
        {
            await Test();
        });
    }

    partial void OnEnableChanged(bool value)
    {
        if (string.IsNullOrWhiteSpace(Url))
        {
            return;
        }
        ConfigHelper.SetSourceEnable(Url, value);
    }

    partial void OnUrlChanged(string? oldValue, string? newValue)
    {
        if (string.IsNullOrWhiteSpace(oldValue))
        {
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                ConfigHelper.AddSource(this);
                return;
            }
            return;
        }
        else if (string.IsNullOrWhiteSpace(newValue))
        {
            _top.Remove(this, oldValue);
            return;
        }
        if (ConfigHelper.HaveSource(newValue))
        {
            Url = oldValue;
        }
        ConfigHelper.SetSourceUrl(oldValue, newValue);
    }

    [RelayCommand]
    public async Task Test()
    {
        IsWork = App.Lang("PluginSourceControl.Info1");
        try
        {
            var data = await HttpUtils.Client.GetStringAsync(Url);
            var obj = JsonConvert.DeserializeObject<PluginDownloadObj>(data);
            if (obj != null && obj.Plugins?.Count > 0)
            {
                IsWork = App.Lang("PluginSourceControl.Info2");
            }
            else
            {
                IsWork = App.Lang("PluginSourceControl.Info3");
            }
        }
        catch
        {
            IsWork = App.Lang("PluginSourceControl.Info3");
        }
    }
}
