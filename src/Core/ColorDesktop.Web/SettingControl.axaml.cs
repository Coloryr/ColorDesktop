using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ColorDesktop.Web;

public partial class SettingControl : UserControl
{
    public SettingControl()
    {
        InitializeComponent();

        DataContext = new SettingModel();
    }
}