using System.ComponentModel;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.MonitorPlugin;

public partial class MonitorControl : UserControl, IInstance
{
    public MonitorControl()
    {
        InitializeComponent();

        var model = new MonitorModel();
        model.PropertyChanged += Model_PropertyChanged;
        DataContext = model;
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == MonitorModel.PanelName)
        {
            var model = (DataContext as MonitorModel)!;

            if (View1.Child is Panel panel)
            {
                panel.Children.Clear();
            }

            switch (model.PanelType)
            {
                case PanelType.Panel:
                    if (View1.Child is StackPanel or WrapPanel)
                    {
                        View1.Child = new Panel();
                    }
                    break;
                case PanelType.Stack:
                    if (View1.Child is not StackPanel)
                    {
                        View1.Child = new StackPanel();
                    }
                    break;
                case PanelType.Wrap:
                    if (View1.Child is not WrapPanel)
                    {
                        View1.Child = new WrapPanel();
                    }
                    break;
            }

            if (View1.Child is Panel panel1)
            {
                foreach (var item in model.Items)
                {
                    panel1.Children.Add(new MonitorItemControl()
                    {
                        DataContext = item
                    });
                }
            }
        }
    }

    public Control CreateView()
    {
        return this;
    }

    public void RenderTick(IInstanceWindow window)
    {
        if (DataContext is MonitorModel model)
        {
            model.Tick();
        }
    }

    public void Start(IInstanceWindow window)
    {

    }

    public void Stop(IInstanceWindow window)
    {

    }

    public void Update(InstanceDataObj obj)
    {
        var config = MonitorPlugin.GetConfig(obj);
        if (DataContext is MonitorModel model)
        {
            model.Update(config);
        }
    }

    public IInstanceHandel? GetHandel()
    {
        return null;
    }

    public void WindowLoaded(IInstanceWindow window)
    {
        RenderTick(window);
    }
}