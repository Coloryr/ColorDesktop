using Avalonia.Controls;
using ColorDesktop.Api;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.PGLauncherPlugin.ColorMC;

public partial class PGColorMCSettingModel : ObservableObject
{
    [ObservableProperty]
    private string? _colorMC;

    public PGColorMCSettingModel()
    {
        if (PGColorMCPlugin.Config == null)
        {
            return;
        }
        _colorMC = PGColorMCPlugin.Config.ColorMC;
    }

    partial void OnColorMCChanged(string? value)
    {
        PGColorMCPlugin.Config.ColorMC = value;
        PGColorMCPlugin.SaveConfig();
    }

    [RelayCommand]
    public async Task SelectFile(Control? control)
    {
        var file = await SystemUtils.SelectFile(TopLevel.GetTopLevel(control),
                LangApi.GetLang("PGColorMCSetting.Info1"), 
                SystemInfo.Os == OsType.Windows ? ["*.exe"] : [], "ColorMC");
        if (file == null || file.Count == 0)
        {
            return;
        }

        var item = file[0].GetPath();
        if (item == null || !SystemUtils.IsExecutable(item))
        {
            return;
        }

        ColorMC = item;
    }
}
