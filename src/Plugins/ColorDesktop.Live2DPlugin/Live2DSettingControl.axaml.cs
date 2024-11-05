using Avalonia.Controls;

namespace ColorDesktop.Live2DPlugin;

public partial class Live2DSettingControl : UserControl
{
    public Live2DSettingControl()
    {
        InitializeComponent();

        DataContext = new Live2DSettingModel();
    }
}