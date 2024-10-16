using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

namespace ColorDesktop.BmPlugin;

public partial class BmInstanceSettingControl : UserControl
{
    public BmInstanceSettingControl()
    {
        InitializeComponent();
    }

    public BmInstanceSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new BmInstanceSettingModel(obj);
    }
}