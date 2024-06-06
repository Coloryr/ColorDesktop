using System.ComponentModel;
using Avalonia.Controls;
using ColorDesktop.Launcher.ViewModels;
using ColorDesktop.Launcher.ViewModels.Main;

namespace ColorDesktop.Launcher.Views;

public partial class MainWindow : Window
{
    private readonly SettingControl _setting = new();
    private readonly PluginControl _plugins = new();
    private readonly InstanceControl _instances = new();
    public MainWindow()
    {
        InitializeComponent();

        Closing += MainWindow_Closing;

        var model = new MainViewModel();
        DataContext = model;
        model.PropertyChanged += Model_PropertyChanged;
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "NowView")
        {
            if (DataContext is MainViewModel model)
            {
                switch (model.NowView)
                {
                    case 0:
                        View1.Child = _setting;
                        break;
                    case 1:
                        View1.Child = _plugins;
                        break;
                    case 2:
                        View1.Child = _instances;
                        break;
                }
            }
        }
    }

    private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }
}
