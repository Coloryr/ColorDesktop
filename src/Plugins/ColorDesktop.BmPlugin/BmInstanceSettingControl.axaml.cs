using Avalonia.Controls;
using ColorDesktop.Api.Objs;

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