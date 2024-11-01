using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

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