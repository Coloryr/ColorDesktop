using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

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