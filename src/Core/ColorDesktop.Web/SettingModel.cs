using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.Web;

public partial class SettingModel : ObservableObject
{
    [ObservableProperty]
    private string _text;

    [RelayCommand]
    public void Reload()
    {
        PluginManager.Reload();

        Text = "已重载浏览器组件";
    }
}
