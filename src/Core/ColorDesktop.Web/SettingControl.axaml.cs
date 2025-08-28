using Avalonia.Controls;

namespace ColorDesktop.Web;

public partial class SettingControl : UserControl
{
    public SettingControl()
    {
        InitializeComponent();

        DataContext = new SettingModel();
    }
}