using Avalonia.Controls;
using ColorDesktop.Api.Objs;

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