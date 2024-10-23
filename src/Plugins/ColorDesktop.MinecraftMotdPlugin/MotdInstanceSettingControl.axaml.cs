using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

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