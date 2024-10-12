using Avalonia.Controls;

namespace ColorDesktop.PGLauncherPlugin.ColorMC;

public partial class PGColorMCSettingControl : UserControl
{
    public PGColorMCSettingControl()
    {
        InitializeComponent();

        DataContext = new PGColorMCSettingModel();
    }
}