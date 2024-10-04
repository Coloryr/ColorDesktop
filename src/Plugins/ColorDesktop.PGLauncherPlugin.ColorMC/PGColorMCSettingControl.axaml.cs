using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ColorDesktop.PGLauncherPlugin.ColorMC;

public partial class PGColorMCSettingControl : UserControl
{
    public PGColorMCSettingControl()
    {
        InitializeComponent();

        DataContext = new PGColorMCSettingModel();
    }
}