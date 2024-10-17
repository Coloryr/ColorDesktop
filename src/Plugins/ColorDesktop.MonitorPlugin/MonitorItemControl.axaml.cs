using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.MonitorPlugin.Controls;
using ColorDesktop.MonitorPlugin.Models;

namespace ColorDesktop.MonitorPlugin;

public partial class MonitorItemControl : UserControl
{
    public MonitorItemControl()
    {
        InitializeComponent();

        DataContextChanged += MonitorItemControl_DataContextChanged;
    }

    private void MonitorItemControl_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is MonitorItemModel model)
        {
            model.PropertyChanged += Model_PropertyChanged;
            InitView();
            model.Update();
        }
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MonitorItemModel.Update))
        {
            var model = (DataContext as MonitorItemModel)!;
            (View1.Child?.DataContext as IUpdate)?.Update(model);
        }
        else if (e.PropertyName == nameof(MonitorItemModel.Reload))
        {
            var model = (DataContext as MonitorItemModel)!;
            (View1.Child?.DataContext as IUpdate)?.Reload(model);
        }
    }

    private void InitView()
    {
        var model = (DataContext as MonitorItemModel)!;
        if (model.MonitorDisplay == MonitorDisplayType.Text
            && View1.Child is not TextViewControl)
        {
            View1.Child = new TextViewControl()
            {
                DataContext = new TextViewModel(model)
            };
        }
    }
}