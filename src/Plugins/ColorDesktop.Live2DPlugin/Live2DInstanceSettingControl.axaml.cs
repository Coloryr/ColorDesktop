using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

namespace ColorDesktop.Live2DPlugin;

public partial class Live2DInstanceSettingControl : UserControl
{
    public Live2DInstanceSettingControl()
    {
        InitializeComponent();
    }

    public Live2DInstanceSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new Live2DInstanceSettingModel(obj);
    }
}