using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ColorDesktop.Live2DPlugin;

public partial class Live2DSettingControl : UserControl
{
    public Live2DSettingControl()
    {
        InitializeComponent();

        DataContext = new Live2DSettingModel();
    }
}