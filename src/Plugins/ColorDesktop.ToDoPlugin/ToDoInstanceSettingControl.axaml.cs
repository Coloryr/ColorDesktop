using Avalonia.Controls;
using ColorDesktop.Api.Objs;

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