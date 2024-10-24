using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api;

namespace ColorDesktop.ToDoPlugin;

public partial class ToDoInstanceSettingControl : UserControl
{
    public ToDoInstanceSettingControl()
    {
        InitializeComponent();
    }

    public ToDoInstanceSettingControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new ToDoInstanceSettingModel(obj);
    }
}