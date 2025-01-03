using Avalonia.Controls;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.MusicControlPlugin;

public partial class MusicInstanceSettingControl : UserControl
{
    public MusicInstanceSettingControl()
    {
        InitializeComponent();
    }

    public MusicInstanceSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new MusicInstanceSettingModel(obj);
    }
}