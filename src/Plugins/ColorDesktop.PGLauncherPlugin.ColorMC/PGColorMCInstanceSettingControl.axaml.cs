using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

namespace ColorDesktop.PGLauncherPlugin.ColorMC;

public partial class PGColorMCInstanceSettingControl : UserControl
{
    public PGColorMCInstanceSettingControl()
    {
        InitializeComponent();
    }

    public PGColorMCInstanceSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new PGColorMCInstanceSettingModel(obj);
    }
}