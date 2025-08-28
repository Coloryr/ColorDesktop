using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Threading;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.UI.Models.Dialog;
using ColorDesktop.Launcher.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
        IsWork = LangApi.GetLang("PluginSourceControl.Info1");
        try
        {
            var data = await LauncherUtils.Client.GetStringAsync(Url);
            var obj = JsonSerializer.Deserialize(data, JsonGen.Default.PluginDownloadObj);
            if (obj != null && obj.Plugins?.Count > 0)
            {
                IsWork = LangApi.GetLang("PluginSourceControl.Info2");
            }
            else
            {
                IsWork = LangApi.GetLang("PluginSourceControl.Info3");
            }
        }
        catch
        {
            IsWork = LangApi.GetLang("PluginSourceControl.Info3");
        }
    }
}
