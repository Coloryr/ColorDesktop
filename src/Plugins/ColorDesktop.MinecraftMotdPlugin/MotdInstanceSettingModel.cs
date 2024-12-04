using ColorDesktop.Api.Objs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.MinecraftMotdPlugin;

public partial class MotdInstanceSettingModel : ObservableObject
{
    [ObservableProperty]
    private string _ip;
    [ObservableProperty]
    private ushort? _port;

    private readonly InstanceDataObj _obj;
    private readonly MotdInstanceObj _config;

    public MotdInstanceSettingModel(InstanceDataObj obj)
    {
        _obj = obj;
        _config = MinecraftMotdPlugin.GetConfig(obj);

        _ip = _config.IP;
        _port = _config.Port;
    }

    partial void OnPortChanged(ushort? value)
    {
        _config.Port = value;
        MinecraftMotdPlugin.SaveConfig(_obj, _config);
    }

    partial void OnIpChanged(string value)
    {
        _config.IP = value;
        MinecraftMotdPlugin.SaveConfig(_obj, _config);
    }
}
