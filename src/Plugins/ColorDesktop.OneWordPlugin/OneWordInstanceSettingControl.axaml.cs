using Avalonia.Controls;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.OneWordPlugin;

public partial class OneWordInstanceSettingControl : UserControl
{
    public OneWordInstanceSettingControl()
    {
        InitializeComponent();
    }

    public OneWordInstanceSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new OneWordInstanceModel(obj);
    }
}