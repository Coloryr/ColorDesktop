using Avalonia.Controls;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.MinecraftMotdPlugin;

public partial class MotdInstanceSettingControl : UserControl
{
    public MotdInstanceSettingControl()
    {
        InitializeComponent();
    }

    public MotdInstanceSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new MotdInstanceSettingModel(obj);
    }
}