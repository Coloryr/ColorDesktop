using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

namespace ColorDesktop.MinecraftSkinPlugin;

public partial class SkinInstanceSettingControl : UserControl
{
    public SkinInstanceSettingControl()
    {
        InitializeComponent();
    }

    public SkinInstanceSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new SkinInstanceSettingModel(obj);
    }
}