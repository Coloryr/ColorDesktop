using Avalonia.Controls;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.CalendarPlugin;

public partial class CalendarInstanceSettingControl : UserControl
{
    public CalendarInstanceSettingControl()
    {
        InitializeComponent();
    }

    public CalendarInstanceSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new CalendarInstanceSettingModel(obj);
    }
}