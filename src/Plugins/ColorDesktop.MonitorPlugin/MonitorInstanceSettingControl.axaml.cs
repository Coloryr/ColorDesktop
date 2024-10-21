using Avalonia.Controls;
using Avalonia.LogicalTree;
using ColorDesktop.Api;

namespace ColorDesktop.MonitorPlugin;

public partial class MonitorInstanceSettingControl : UserControl
{
    public MonitorInstanceSettingControl()
    {
        InitializeComponent();
    }

    public MonitorInstanceSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new MonitorInstanceSettingModel(obj);
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);

        if (DataContext is MonitorInstanceSettingModel model)
        {
            model.Stop();
        }
    }
}