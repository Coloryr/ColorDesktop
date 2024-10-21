using System.ComponentModel;
using Avalonia.Controls;
using ColorDesktop.MonitorPlugin.Controls;
using ColorDesktop.MonitorPlugin.Models;

namespace ColorDesktop.MonitorPlugin;

public partial class MonitorItemControl : UserControl
{
    private MonitorItemModel _model;

    public MonitorItemControl()
    {
        InitializeComponent();

        DataContextChanged += MonitorItemControl_DataContextChanged;
    }

    private void MonitorItemControl_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is MonitorItemModel model)
        {
            _model = model;
            model.PropertyChanged += Model_PropertyChanged;
            InitView();
            model.Update();
        }
        else
        {
            if (_model != null)
            {
                _model.PropertyChanged -= Model_PropertyChanged;
                _model = null;
            }
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
        else if (model.MonitorDisplay == MonitorDisplayType.ProgressBar1
            && View1.Child is not ProgressBar1Control)
        {
            View1.Child = new ProgressBar1Control()
            {
                DataContext = new ProgressBarModel(model)
            };
        }
        else if (model.MonitorDisplay == MonitorDisplayType.ProgressBar2
            && View1.Child is not ProgressBar2Control)
        {
            View1.Child = new ProgressBar2Control()
            {
                DataContext = new ProgressBarModel(model)
            };
        }
        else if (model.MonitorDisplay == MonitorDisplayType.ProgressBar3
            && View1.Child is not ProgressBar3Control)
        {
            View1.Child = new ProgressBar3Control()
            {
                DataContext = new ProgressBar3Model(model)
            };
        }
        else if (model.MonitorDisplay == MonitorDisplayType.ProgressBar4
            && View1.Child is not ProgressBar4Control)
        {
            View1.Child = new ProgressBar4Control()
            {
                DataContext = new ProgressBar3Model(model)
            };
        }
        else if (model.MonitorDisplay == MonitorDisplayType.ProgressBar5
            && View1.Child is not ProgressBar5Control)
        {
            View1.Child = new ProgressBar5Control()
            {
                DataContext = new ProgressBar5Model(model)
            };
        }
    }
}