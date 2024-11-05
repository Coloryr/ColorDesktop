using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.CoreLib;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Live2DCSharpSDK.Framework.Core;

namespace ColorDesktop.Live2DPlugin;

public partial class Live2DSettingModel : ObservableObject
{
    [ObservableProperty]
    private string _version;
    [ObservableProperty]
    private string _info;

    [ObservableProperty]
    private bool _haveCore;
    [ObservableProperty]
    private bool _haveInfo;

    public Live2DSettingModel()
    {
        Reload();
    }

    [RelayCommand]
    public void OpenUrl()
    {
        CoreHelper.OpUrl("https://www.live2d.com/download/cubism-sdk/download-native/");
    }

    [RelayCommand]
    public async Task Import(Control? control)
    {
        var file = await CoreHelper.SelectFile(TopLevel.GetTopLevel(control),
            LangApi.GetLang("Live2DSettingControl.Info1"), ["*.zip"],
            "CubismSdkForNative");
        if (file == null || file.Count == 0)
        {
            return;
        }

        var item = file[0].GetPath();
        if (!File.Exists(item))
        {
            return;
        }

        bool res = await Task.Run(() =>
        {
            return Live2DPlugin.SetLive2DCore(item);
        });

        if (!res)
        {
            Info = LangApi.GetLang("Live2DSettingControl.Error2");
            HaveInfo = true;
        }
        else
        {
            Info = LangApi.GetLang("Live2DSettingControl.Info3");
            HaveCore = true;
            HaveInfo = true;
        }
    }

    private void Reload()
    {
        if (Live2DPlugin.IsCoreLoad)
        {
            try
            {
                var version = CubismCore.Version();

                uint major = (version & 0xFF000000) >> 24;
                uint minor = (version & 0x00FF0000) >> 16;
                uint patch = version & 0x0000FFFF;
                uint vesionNumber = version;

                Version = $"{major:0}.{minor:0}.{patch:0000} ({vesionNumber})";
                HaveCore = true;
            }
            catch
            {
                Version = LangApi.GetLang("Live2DSettingControl.Error1");
                HaveCore = false;
            }
        }
        else
        {
            Version = LangApi.GetLang("Live2DSettingControl.Error1");
            HaveCore = false;
        }
    }
}
