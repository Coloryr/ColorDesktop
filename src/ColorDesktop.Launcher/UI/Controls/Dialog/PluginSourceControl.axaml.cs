using System;
using System.ComponentModel;
using Avalonia.Controls;
using ColorDesktop.Launcher.UI.Models.Dialog;

namespace ColorDesktop.Launcher.UI.Controls.Dialog;

public partial class PluginSourceControl : UserControl
{
    public PluginSourceControl()
    {
        InitializeComponent();

        DataContextChanged += PluginSourceControl_DataContextChanged;
    }

    private void PluginSourceControl_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is PluginSourceModel model)
        {
            model.PropertyChanged += Model_PropertyChanged;
        }
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == PluginSourceModel.GoEditName)
        {
            DataGrid1.CurrentColumn = DataGrid1.Columns[1];
            DataGrid1.BeginEdit();
        }
    }
}