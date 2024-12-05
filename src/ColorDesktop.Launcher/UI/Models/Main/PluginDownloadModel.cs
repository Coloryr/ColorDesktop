using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AvaloniaEdit.Utils;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Objs;
using ColorDesktop.Launcher.UI.Models.Dialog;
using ColorDesktop.Launcher.UI.Models.Items;
using ColorDesktop.Launcher.UI.Windows;
using ColorDesktop.Launcher.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using Newtonsoft.Json;

namespace ColorDesktop.Launcher.UI.Models.Main;

public partial class MainViewModel
{
    public ObservableCollection<PluginDownloadItemModel> Downloads { get; init; } = [];

    public List<PluginDownloadItemModel> _downloads { get; init; } = [];

    [ObservableProperty]
    private string _downloadText;

    partial void OnDownloadTextChanged(string value)
    {
        LoadDownloadList();
    }

    [RelayCommand]
    public void EditSource()
    {
        var model = new PluginSourceModel();
        DialogHost.Show(model, MainWindow.DialogHostName);
    }

    [RelayCommand]
    public void SourceReload()
    {
        GetDownloadList();
    }

    private async void GetDownloadList()
    {
        _ = DialogHost.Show(LangApi.GetLang("MainWindow.Info12"), MainWindow.DialogHostName);
        _downloads.Clear();

        foreach (var item in ConfigHelper.Config.PluginSource)
        {
            if (!item.Enable || string.IsNullOrWhiteSpace(item.Url)
                || !item.Url.StartsWith("http"))
            {
                continue;
            }
            try
            {
                var data = await LauncherUtils.Client.GetStringAsync(item.Url);
                var obj = JsonConvert.DeserializeObject<PluginDownloadObj>(data);
                if (obj == null || obj.Plugins == null)
                {
                    continue;
                }
                foreach (var item1 in obj.Plugins)
                {
                    _downloads.Add(new(this, item1, obj.Source, obj.BaseUrl));
                }
            }
            catch
            {

            }
        }

        LoadDownloadList();

        DialogHost.Close(MainWindow.DialogHostName);
    }

    private void LoadDownloadList()
    {
        Downloads.Clear();
        if (string.IsNullOrWhiteSpace(DownloadText))
        {
            Downloads.AddRange(_downloads);
        }
        else
        {
            Downloads.AddRange(_downloads.Where(item => item.Name.Contains(DownloadText)));
        }
    }
}
