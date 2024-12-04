using Avalonia.Controls;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.PGLauncherPlugin;

public partial class PGLauncherInstanceSettingControl : UserControl
{
    public PGLauncherInstanceSettingControl()
    {
        InitializeComponent();
    }

    public PGLauncherInstanceSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new PGLauncherInstanceSettingModel(obj);
    }
}