using Avalonia.Controls;
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