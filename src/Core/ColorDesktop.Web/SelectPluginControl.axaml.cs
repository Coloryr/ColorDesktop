using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.Web;

public partial class SelectPluginControl : UserControl
{
    public SelectPluginControl()
    {
        InitializeComponent();
    }

    public SelectPluginControl(InstanceDataObj obj)
    {
        InitializeComponent();

        DataContext = new SelectPluginModel(obj);
    }
}