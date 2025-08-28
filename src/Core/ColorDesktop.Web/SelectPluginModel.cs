using ColorDesktop.Api.Objs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.Web;

public partial class SelectPluginModel(InstanceDataObj obj) : ObservableObject
{
    public string[] Plugins { get; init; } = PluginManager.GetPlugins();

    [ObservableProperty]
    private string _plugin;

    private readonly WebInstanceObj _config = new();

    partial void OnPluginChanged(string value)
    {
        _config.Plugin = value;
        obj.SaveConfig(_config, "webplugin.json", JsonGen.Default.WebInstanceObj);
    }
}
