using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api.Objs;

namespace PluginDemo;

public partial class DemoInstanceSettingControl : UserControl
{
    public DemoInstanceSettingControl()
    {
        InitializeComponent();
    }

    public DemoInstanceSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new DemoInstanceSettingModel(obj);
    }
}